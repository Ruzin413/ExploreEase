using Microsoft.AspNetCore.Identity;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class GetUser
        //: IGetUsers
    {
        public UserManager<ExploreEaseUser> _userManager;
        public GetUser(UserManager<ExploreEaseUser> _userManager)
        {
           this._userManager = _userManager;
        }
        public List<ExploreEaseUser> GetUsers()
        {
            var users = _userManager.Users.ToList();
            return users;

        }
    }
}
