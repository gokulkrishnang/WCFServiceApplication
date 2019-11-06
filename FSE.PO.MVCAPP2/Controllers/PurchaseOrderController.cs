using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//additional Namespace
using FSE.PO.MVCAPP.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
//using FSE.PO.MVCAPP.PurchaseOrderService;
//using PurchaseOrderEntity = FSE.PO.MVCAPP.PurchaseOrderService.PurchaseOrderEntity;

namespace FSE.PO.MVCAPP.Controllers
{
    public class PurchaseOrderController : Controller
    {
        //PurchaseOrderServiceClient purchaseOrderServiceClient = null;

        HttpClient _client = null;
        //Hosted web API REST Service base url        
        string Baseurl = "http://localhost:52845/";

        public PurchaseOrderController()
        {
            var client = new HttpClient();

            //Passing service base url
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _client = client;
        }

        /// <summary>
        /// First Action method called when page loads
        /// Fetch all the records/PurchaseOrder from PurchaseOrderservice 
        /// and display it
        /// </summary>
        /// <returns>Index View</returns>
        public async Task<ActionResult> Index()
        {
            List<PurchaseOrder> purchaseOrder = new List<PurchaseOrder>();

            if (_client != null)
            {
                //Sending request to find web api REST service resource GetAllPurchaseOrder using HttpClient
                HttpResponseMessage Res = await _client.GetAsync("api/PurchaseOrder/GetAllPurchaseOrder");

                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api 
                    var poResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list
                    purchaseOrder = JsonConvert.DeserializeObject<List<PurchaseOrder>>(poResponse);
                }
            }

            //returning the PurchaseOrder list to view
            return View("Index", purchaseOrder);
        }

        // POST: Product/Create
        [HttpPost]
        public async Task<ActionResult> Insert(PurchaseOrder purchaseOrder)
        {
            //int result = purchaseOrderServiceClient.CreatePurchaseOrder(purchaseOrder);

            if (_client != null)
            {
                //Sending request to find web api REST service resource POST a PurchaseOrder using HttpClient
                var response = await _client.PostAsync("api/PurchaseOrder/CreatePurchaseOrder", new StringContent(
                    new JavaScriptSerializer().Serialize(purchaseOrder), Encoding.UTF8, "application/json"));

                //Checking the response is successful or not which is sent using HttpClient
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            //pass the action method as 'Index' and redirect to Index action (calling Index page)
            return RedirectToAction("Index");
        }

       //this action method is of type HttpGet
       //using this action method we will create a new form to enter new products
       // GET: Product/Create
       public async Task<ActionResult> Insert()
       {
            //Sending request to find web api REST service resource GetAllSuppliers using HttpClient
            HttpResponseMessage httpResMsgSuppliers = await _client.GetAsync("api/PurchaseOrder/GetSupplier");
            HttpResponseMessage httpResMsgItems = await _client.GetAsync("api/PurchaseOrder/GetItem");

            ViewBag.Suppliers = ToSelectList(allSuppliers(httpResMsgSuppliers), "SUPLNO", "SUPLNAME");
            ViewBag.Items = ToSelectList(allSuppliers(httpResMsgItems), "ITCODE", "ITDESC");

           return View(new PurchaseOrder());
       }

        private List<Supplier> allSuppliers(HttpResponseMessage Res)
        {
            List<Supplier> supplier = new List<Supplier>();           

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api 
                var allSuppliers = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                supplier = JsonConvert.DeserializeObject<List<Supplier>>(allSuppliers);
            }

            return supplier;
        }

        private List<Item> allItems(HttpResponseMessage Res)
        {
            List<Item> item = new List<Item>();

            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api 
                var allItems = Res.Content.ReadAsStringAsync().Result;

                //Deserializing the response recieved from web api and storing into the Employee list
                item = JsonConvert.DeserializeObject<List<Item>>(allItems);
            }

            return item;
        }

        // GET: PurchaseOrder/Delete/P001
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> Delete(string poNo)
        {
            //int result = purchaseOrderServiceClient.DeletePurchaseOrder(poNo);
            var response = await _client.DeleteAsync($"api/PurchaseOrder/DeletePurchaseOrder/{poNo}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            //after the record is deleted from the database redirect to Index action method
            return RedirectToAction("Index");
        }

        
        // GET: PurchaseOrder/Edit/P001
        public async Task<ActionResult> Edit(string poNo)
        {
            var result = await _client.GetAsync($"api/PurchaseOrder/GetPurchaseOrderByPONO/{poNo}");
            PurchaseOrder purchaseOrder = null;
            if (result.IsSuccessStatusCode)
            {
                purchaseOrder = await result.Content.ReadAsAsync<PurchaseOrder>();
            }
            return View(purchaseOrder);
        }
        
        // POST: Product/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(PurchaseOrder purchaseOrder)
        {
            
            var response = await _client.PostAsync("api/PurchaseOrder/UpdatePurchaseOrder", new StringContent(
                new JavaScriptSerializer().Serialize(purchaseOrder), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }       

        [NonAction]
        public SelectList ToSelectList(List<Supplier> supplier, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Supplier entity in supplier)
            {
                list.Add(new SelectListItem()
                {
                    Text = entity.SuplName.ToString(),
                    Value = entity.SuplNo.ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }

        [NonAction]
        public SelectList ToSelectList(List<Item> item, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (Item entity in item)
            {
                list.Add(new SelectListItem()
                {
                    Text = entity.ItDesc.ToString(),
                    Value = entity.ItCode.ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }
    }
}