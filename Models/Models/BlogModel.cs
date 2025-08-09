using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        [StringLength(10000)]
        public string Name { get; set; }
        [StringLength(10000)]
        public string Email { get; set; }
        [StringLength(10000)]
        public string Description { get; set; }
        [StringLength(100000)]
        public string Blogimage { get; set; } 
        public int Likes { get; set; }
        public bool likestatus { get; set; }
    }
}
