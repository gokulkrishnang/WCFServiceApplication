using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//additional Namespace
using FSE.PO.WEBAPI.Models;
using FSE.PO.WEBAPI.Repositories;

namespace FSE.PO.WEBAPI.Controllers
{
    public class PurchaseOrderController : ApiController
    {
        readonly IPurchaseOrderRepository repository = new PurchaseOrderRepository();

        //GetAllPurchaseOrder
        //GET api/GetAllPurchaseOrder
        [HttpGet]
        [Route("api/PurchaseOrder/GetAllPurchaseOrder")]
        public IEnumerable<PurchaseOrder> Get()
        {
            return repository.GetAll();
        }

        //GetPurchaseOrderByPONO
        //GET api/GetPurchaseOrderByPONO/P004
        [HttpGet]
        [Route("api/PurchaseOrder/GetPurchaseOrderByPONO/{poNo?}")]
        public PurchaseOrder Get(string poNo)
        {
            PurchaseOrder purchaseOrder = repository.GetData(poNo);
            if (purchaseOrder == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return purchaseOrder;
        }

        //CreatePurchaseOrder
        //POST api/PurchaseOrder/CreatePurchaseOrder
        [HttpPost]
        [Route("api/PurchaseOrder/CreatePurchaseOrder")]
        public int Insert(PurchaseOrder purchaseOrder)
        {
            return repository.Insert(purchaseOrder);
        }

        //DeletePurchaseOrder
        [Route("api/PurchaseOrder/DeletePurchaseOrder")]
        public int Delete(string poNo)
        {
            return repository.Delete(poNo);
        }

        //UpdatePurchaseOrder
        [Route("api/PurchaseOrder/UpdatePurchaseOrder")]
        public int Put(PurchaseOrder purchaseOrder)
        {
            return repository.Update(purchaseOrder);
        }

        //GetSupplier
        [HttpGet]
        [Route("api/PurchaseOrder/GetSupplier")]
        public IEnumerable<Supplier> GetSupplier()
        {
            return repository.GetSupplier();
        }

        //GetItem
        [HttpGet]
        [Route("api/PurchaseOrder/GetItem")]
        public IEnumerable<Item> GetItem()
        {
            return repository.GetItem();
        }
    }
}
