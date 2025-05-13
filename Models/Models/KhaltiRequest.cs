using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class KhaltiRequest
    {
        public string return_url { get; set; }
        public string website_url { get; set; }
        public long amount { get; set; } // amount in paisa (multiply by 100)
        public string purchase_order_id { get; set; }
        public string purchase_order_name { get; set; }
        public CustomerInfo customer_info { get; set; }
    }
}
