using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class BookmarkModel
    {
        public int Id { get; set; }
        [StringLength(10000)]
        public string userEmail { get; set; }
        [StringLength(10000)]
        public int TourPackageId {  get; set; }
    }
}
