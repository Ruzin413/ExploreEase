using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Commentmodel
    {
        public int id { get; set; }
        public int BlogId { get; set; }
        [StringLength(10000)]
        public string name { get; set; }
        [StringLength(10000)]
        public string email { get; set; }
        [StringLength(10000)]
        public string comment { get; set; }
       
    }
}
