using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//additional Namespace
using System.Runtime.Serialization;

namespace FSE.PO.SVCAPP.Entity
{
    [DataContract]
    public class SupplierEntity
    {
        [DataMember]
        public string SuplNo;

        [DataMember]
        public string SuplName;

        [DataMember]
        public string SuplAddr;
    }
}