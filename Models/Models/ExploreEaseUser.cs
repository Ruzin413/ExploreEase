﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ExploreEaseUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
