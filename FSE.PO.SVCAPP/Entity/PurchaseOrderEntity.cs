using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//additional Namespace
using System.Runtime.Serialization;

namespace FSE.PO.SVCAPP.Entity
{
    [DataContract]
    public class PurchaseOrderEntity
    {
        [DataMember]
        public string SuplNo;

        [DataMember]
        public string SuplName;

        [DataMember]
        public string ITCode;

        [DataMember]
        public string ITDesc;

        [DataMember]
        public int Qty;

        [DataMember]
        public string PONo;

        [DataMember]
        public DateTime PODate;

    }
}