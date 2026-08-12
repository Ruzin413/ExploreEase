using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IBlogRepo
    {
        Task<bool> PostBlog(BlogModel model);
        Task<List<BlogModel>> GetBlogs(string currentUserId);
        Task<bool> LikeUpdate(int blogId, string userId);
        Task<bool> LikeUpdate(BlogLikes model);
        Task<bool> unLikeUpdate(int blogId, string username);
        Task<bool> PostComment(Commentmodel model);
        Task<List<Commentmodel>> GetComments(int Blogid);
        Task<bool> DeleteBlog(int blogId);
    }
}
