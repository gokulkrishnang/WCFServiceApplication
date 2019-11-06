using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//additionalNameSpace
using System.ComponentModel.DataAnnotations;

namespace FSE.PO.MVCAPP.Models
{
    public class PurchaseOrderEntity2
    {
        [Display(Name = "Supplier Number")]
        [Required(ErrorMessage = "Supplier Number is required.")]
        public string SuplNo { get; set; }

        [Display(Name = "Supplier Name")]
        [Required(ErrorMessage = "Supplier Name is required.")]
        public string SuplName { get; set; }

        [Display(Name = "Item Code")]
        [Required(ErrorMessage = "Item Code is required.")]
        public string ITCode { get; set; }

        [Display(Name = "Item Description")]
        [Required(ErrorMessage = "Item Description is required.")]
        public string ITDesc { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Item Quantity is required.")]
        public int Qty { get; set; }

        [Display(Name = "PO Number")]
        [Required(ErrorMessage = "Purchase Order Number is required.")]
        public string PONo;

        [Display(Name = "PO Date")]
        [Required(ErrorMessage = "Purchase Order Date is required.")]
        public DateTime PODate;
    }
}