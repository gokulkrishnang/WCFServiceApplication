using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//additional Namespace
using System.Runtime.Serialization;

namespace FSE.PO.SVCAPP.Entity
{
    [DataContract]
    public class ItemEntity
    {
        [DataMember]
        public string ItCode;

        [DataMember]
        public string ItDesc;

        [DataMember]
        public decimal ItRate;

    }
}