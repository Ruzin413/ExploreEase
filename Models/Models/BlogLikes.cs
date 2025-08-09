using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class BlogLikes
    {
        public int Id { get; set; }
        public int Blogid {  get; set; }
        [StringLength(10000)]
        public string Name { get; set; }
        public DateTime time { get; set; }
    }
}
