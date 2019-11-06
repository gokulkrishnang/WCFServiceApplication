using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
//additional Namespace
using FSE.PO.SVCAPP.Entity;

namespace FSE.PO.SVCAPP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPurchaseOrderService" in both code and config file together.
    [ServiceContract]
    public interface IPurchaseOrderService
    {
        [OperationContract]
        string ValidateService();

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        List<PurchaseOrderEntity> GetAllPurchaseOrder();

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        PurchaseOrderEntity GetPurchaseOrderByPONO(string poNumber);

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        int CreatePurchaseOrder(PurchaseOrderEntity purchaseOrderEntity);

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        int UpdatePurchaseOrder(PurchaseOrderEntity purchaseOrderEntity);

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        int DeletePurchaseOrder(string poNo);

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        IEnumerable<SupplierEntity> GetSupplier();

        [OperationContract]
        [FaultContract(typeof(ExceptionMessage))]
        IEnumerable<ItemEntity> GetItem();
    }

    
}
