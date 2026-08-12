using Microsoft.AspNetCore.Http;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBlogServices
    {
        Task<bool> Postblog(IFormCollection form, string email, string username);
        Task<List<BlogModel>> GetBlogs(string name);
        Task<bool> LikeUpdate(int id, string username);
        Task<bool> unLikeUpdate(int id, string username);
        Task<bool> PostComment(int BlogId, string username, string email, string CommentText);
        Task<List<Commentmodel>> GetComments(int BlogId);
        Task<bool> DeleteBlog(int blogId);
    }
}
