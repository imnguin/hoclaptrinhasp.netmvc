using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_ASPMVC.Models
{
    public class lstOrder
    {
        public int IdOrder { get; set; }
        public string NameCustomer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string NameProduct { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public DateTime? OrderTime { get; set; }
        public Boolean? PaymentStatus { get; set; }
        public Boolean? OrderStatus { get; set; }
        public string Color { get; set; }
    }
}