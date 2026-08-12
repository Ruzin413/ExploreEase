using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BlogRepo : IBlogRepo
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public BlogRepo(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext = exploreEaseDbContext;
        }

        public async Task<bool> PostBlog(BlogModel model)
        {
            await _exploreEaseDbContext.Blogdb.AddAsync(model);
            var result = await _exploreEaseDbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<BlogModel>> GetBlogs(string currentUserId)
        {
            List<BlogModel> blogs = await _exploreEaseDbContext.Blogdb.ToListAsync();

            var likeGroups = await _exploreEaseDbContext.BlogLikes
                .GroupBy(like => like.Blogid)
                .Select(group => new { Blogid = group.Key, LikeCount = group.Count() })
                .ToListAsync();

            var userLikedBlogIds = await _exploreEaseDbContext.BlogLikes
                .Where(like => like.Name == currentUserId)
                .Select(like => like.Blogid)
                .ToListAsync();

            List<BlogModel> result = new List<BlogModel>();

            foreach (BlogModel blog in blogs)
            {
                int likeCount = 0;
                foreach (var likeGroup in likeGroups)
                {
                    if (likeGroup.Blogid == blog.Id)
                    {
                        likeCount = likeGroup.LikeCount;
                        break;
                    }
                }

                bool likestatus1 = userLikedBlogIds.Contains(blog.Id);

                BlogModel model = new BlogModel
                {
                    Id = blog.Id,
                    Name = blog.Name,
                    Email = blog.Email,
                    Description = blog.Description,
                    Blogimage = blog.Blogimage,
                    Likes = likeCount,
                    likestatus = likestatus1
                };
                result.Add(model);
            }
            return result;
        }

        // New method to add a like only if it doesn't exist yet
        public async Task<bool> LikeUpdate(int blogId, string userId)
        {
            bool alreadyLiked = await _exploreEaseDbContext.BlogLikes
                .AnyAsync(like => like.Blogid == blogId && like.Name == userId);

            if (alreadyLiked)
            {
                // User already liked this blog, do not add again
                return false;
            }

            var newLike = new BlogLikes
            {
                Blogid = blogId,
                Name = userId,
                time = System.DateTime.UtcNow
            };

            await _exploreEaseDbContext.BlogLikes.AddAsync(newLike);
            var result = await _exploreEaseDbContext.SaveChangesAsync();
            return result > 0;
        }

        //Optional: keep your old LikeUpdate but note it doesn't check duplicates
        public async Task<bool> LikeUpdate(BlogLikes model)
        {
            await _exploreEaseDbContext.BlogLikes.AddAsync(model);
            var result = await _exploreEaseDbContext.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> unLikeUpdate(int blogId, string username)
        {
            var like = await _exploreEaseDbContext.BlogLikes
                .FirstOrDefaultAsync(l => l.Blogid == blogId && l.Name == username);
            if (like == null)
            {
                // No like found to remove
                return false;
            }
            _exploreEaseDbContext.BlogLikes.Remove(like);
            var result = await _exploreEaseDbContext.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> PostComment(Commentmodel model)
        {
            await _exploreEaseDbContext.commentdb.AddAsync(model);
            var result = await _exploreEaseDbContext.SaveChangesAsync();
            return result > 0;
        }
        public async Task<List<Commentmodel>> GetComments(int Blogid)
        {
            return await _exploreEaseDbContext.commentdb.Where(x => x.BlogId == Blogid).ToListAsync();
        }
        public async Task<bool> DeleteBlog(int blogId)
        {
            var blog = await _exploreEaseDbContext.Blogdb.FirstOrDefaultAsync(x => x.Id == blogId);
            if (blog == null) return false;

            _exploreEaseDbContext.Blogdb.Remove(blog);
            var result = await _exploreEaseDbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
