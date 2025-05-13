using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class KhaltiPaymentResponse
    {
        public int Id { get; set; } // Primary key
        public string PurchaseOrderId { get; set; }
        public int TourPackageId { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }


}
