using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//additional Namespace
using FSE.PO.WEBAPI.Models;

namespace FSE.PO.WEBAPI.Repositories
{
    public interface IPurchaseOrderRepository
    {
        IEnumerable<PurchaseOrder> GetAll();
        PurchaseOrder GetData(string poNo);
        int Insert(PurchaseOrder purchaseOrder);
        int Delete(string poNo);
        int Update(PurchaseOrder purchaseOrder);
        IEnumerable<Supplier> GetSupplier();
        IEnumerable<Item> GetItem();
    }
}
