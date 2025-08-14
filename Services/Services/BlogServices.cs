using DataAcessLayer.DataAcess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BlogServices
    {
        private readonly BlogRepo _blogRepo;
        private readonly ImageSaveService _imageSaveService;
        public BlogServices(BlogRepo blogRepo, ImageSaveService imageSaveService)
        {
            _blogRepo = blogRepo;
            _imageSaveService = imageSaveService;
        }
        public async Task<bool> Postblog(IFormCollection form, string email, string username)
        {
            var text = form["text"].FirstOrDefault();
            var image = form.Files["image"];  // To get uploaded file, use form.Files

            string imagePath = null;

            if (image != null && image.Length > 0)
            {
                imagePath = await _imageSaveService.SaveImageAsync(image, "BlogImages");
            }

            var blogModel = new BlogModel()
            {
                Name = username,
                Email = email,
                Description = text,
                Blogimage = imagePath,
                Likes = 0,
                likestatus = false
            };
            return await _blogRepo.PostBlog(blogModel);
        }
        public async Task<List<BlogModel>> GetBlogs(string name)
        {

            return await _blogRepo.GetBlogs(name);
        }
        public async Task<bool> LikeUpdate(int id,String username)
        {
            var bloglike = new BlogLikes
            {
                Blogid = id,
                Name = username,
                time = DateTime.Now,
            };
            bool result = await _blogRepo.LikeUpdate(id,username);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> unLikeUpdate(int id, String username)
        {
            bool result = await _blogRepo.unLikeUpdate(id, username);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> PostComment(int BlogId, string username, string email, string CommentText)
        {
            var model2 = new Commentmodel
            {
                BlogId = BlogId,
                name = username,
                email = email,
                comment = CommentText
            };
            return await _blogRepo.PostComment(model2);
        }
        public async Task<List<Commentmodel>> GetComments(int BlogId)
        {
            return await _blogRepo.GetComments(BlogId);
        }
        public async Task<bool> DeleteBlog(int blogId)
        {
            var result = await _blogRepo.DeleteBlog(blogId); 
            if (result) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
