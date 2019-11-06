using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//additional Namespace
using FSE.PO.MVCAPP.Models;
using FSE.PO.MVCAPP.PurchaseOrderService;
using PurchaseOrderEntity = FSE.PO.MVCAPP.PurchaseOrderService.PurchaseOrderEntity;

namespace FSE.PO.MVCAPP.Controllers
{
    public class PurchaseOrderController : Controller
    {
        PurchaseOrderServiceClient purchaseOrderServiceClient = null;

        public PurchaseOrderController()
        {
            PurchaseOrderServiceClient purchaseOrderClient = new PurchaseOrderServiceClient();
            purchaseOrderServiceClient = purchaseOrderClient;
        }

        // GET: PurchaseOrder
        //Implement WCF Service
        public ActionResult ConnectService()
        {
            PurchaseOrderService.PurchaseOrderServiceClient purchaseOrderServiceClient =
                new PurchaseOrderService.PurchaseOrderServiceClient();
            string dataConnectService = purchaseOrderServiceClient.ValidateService();
            return Content(dataConnectService);
        }

        /// <summary>
        /// First Action method called when page loads
        /// Fetch all the records/PurchaseOrder from PurchaseOrderservice 
        /// and display it
        /// </summary>
        /// <returns>Index View</returns>
        public ActionResult Index()
        {
            List<PurchaseOrderService.PurchaseOrderEntity> purchaseOrder = purchaseOrderServiceClient.GetAllPurchaseOrder();
            return View("Index", purchaseOrder);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Insert(PurchaseOrderEntity purchaseOrder)
        {
            int result = purchaseOrderServiceClient.CreatePurchaseOrder(purchaseOrder);

            //pass the action method as 'Index' and redirect to Index action (calling Index page)
            return RedirectToAction("Index");
        }

        //this action method is of type HttpGet
        //using this action method we will create a new form to enter new products
        // GET: Product/Create
        public ActionResult Insert()
        {
            List<SupplierEntity> supplier = purchaseOrderServiceClient.GetSupplier();
            List<ItemEntity> item = purchaseOrderServiceClient.GetItem();

            ViewBag.Suppliers = ToSelectList(supplier, "SUPLNO", "SUPLNAME");
            ViewBag.Items = ToSelectList(item, "ITCODE", "ITDESC");

            return View(new PurchaseOrderEntity());
        }

        // GET: PurchaseOrder/Delete/P001
        public ActionResult Delete(string poNo)
        {
            int result = purchaseOrderServiceClient.DeletePurchaseOrder(poNo);

            //after the record is deleted from the database redirect to Index action method
            return RedirectToAction("Index");
        }

        // GET: PurchaseOrder/Edit/P001
        public ActionResult Edit(string poNo)
        {
            var purchaseOrder = purchaseOrderServiceClient.GetPurchaseOrderByPONO(poNo);

            PurchaseOrderEntity poEntity = new PurchaseOrderEntity();

            List<SupplierEntity> supplier = purchaseOrderServiceClient.GetSupplier();
            List<ItemEntity> item = purchaseOrderServiceClient.GetItem();

            ViewBag.Suppliers = ToSelectList(supplier, "SUPLNO", "SUPLNAME");
            ViewBag.Items = ToSelectList(item, "ITCODE", "ITDESC");

            //if there is a row with given id
            if (purchaseOrder != null)
            {
                poEntity.SuplName = purchaseOrder.SuplName;
                poEntity.PODate = Convert.ToDateTime(purchaseOrder.PODate.ToString());
                poEntity.ITDesc = purchaseOrder.ITDesc;
                poEntity.Qty = purchaseOrder.Qty;

                ViewBag.Suppliers = new SelectList(supplier, "SUPLNO", "SUPLNAME", purchaseOrder.SuplNo);
                ViewBag.Items = new SelectList(item, "ITCODE", "ITDESC", purchaseOrder.ITCode);

                //So far we have not created View for Edit action method, lets create now a view
                return View(poEntity);
            }
            else
                //No matching data hence show the view
                return RedirectToAction("Index");
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(PurchaseOrderEntity purchaseOrder)
        {
            try
            {
                purchaseOrderServiceClient.UpdatePurchaseOrder(purchaseOrder);

                //redirect to Index action method
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public SelectList ToSelectList(List<SupplierEntity> supplier, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (SupplierEntity entity in supplier)
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
        public SelectList ToSelectList(List<ItemEntity> item, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (ItemEntity entity in item)
            {
                list.Add(new SelectListItem()
                {
                    Text = entity.ItDesc.ToString(),
                    Value = entity.ItCode.ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }

        /*
        /// <summary>
        /// Action method, called when the "New Purchase Order" link is clicked
        /// </summary>
        /// <returns>Create View</returns>
        public ActionResult Insert()
        {
            //MD.MobileList = new SelectList(MCon.GetMobileList(), "MobileID", "MobileName"); // model binding
            

            List<SupplierEntity> supplier = purchaseOrderServiceClient.GetSupplier();
            List<ItemEntity> item = purchaseOrderServiceClient.GetItem();

            ViewBag.Suppliers = new SelectList(supplier, "SUPLNO", "SUPLNAME", "0");
            ViewBag.Items = new SelectList(item, "ITCODE", "ITDESC", "0");

            return View("Create");
        }

        /// <summary>
        /// Action method, called when the user hit "Submit" button
        /// </summary>
        /// <param name="frm">Form Collection  Object</param>
        /// <param name="action">Used to differentiate between "submit" and "cancel"</param>
        /// <returns></returns>
        public ActionResult InsertRecord(FormCollection frm, string action)
        {
            if (action == "Submit")
            {
                //CRUDModel model = new CRUDModel();
                //string name = frm["txtName"];
                //int age = Convert.ToInt32(frm["txtAge"]);
                //string gender = frm["gender"];
                var purchaseOrder = new PurchaseOrderService.PurchaseOrderEntity();
                //                    Suppliers
                //PODate
                //Items
                //Quantity
                int status = purchaseOrderServiceClient.CreatePurchaseOrder(purchaseOrder); 
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        */
    }
}