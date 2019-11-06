using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSE.PO.MVCAPP.Models
{
    public class PurchaseOrder
    {
        public string SuplNo { get; set; }

        public string SuplName { get; set; }

        public string ITCode { get; set; }

        public string ITDesc { get; set; }

        public int Qty { get; set; }

        public string PONo { get; set; }

        public DateTime PODate { get; set; }
    }
}