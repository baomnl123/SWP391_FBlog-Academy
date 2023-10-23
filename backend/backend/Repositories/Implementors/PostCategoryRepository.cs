﻿using backend.Models;
using backend.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace backend.Repositories.Implementors
{
    public class PostCategoryRepository : IPostCategoryRepository
    {
        private readonly FBlogAcademyContext _context;

        public PostCategoryRepository()
        {
            _context = new();
        }

        public bool CreatePostCategory(PostCategory postCategory)
        {
            _context.Add(postCategory);
            return Save();
        }

        public bool DisablePostCategory(PostCategory postCategory)
        {
            postCategory.Status = false;
            _context.Update(postCategory);
            return Save();
        }

        public bool EnablePostCategory(PostCategory postCategory)
        {
            postCategory.Status = true;
            _context.Update(postCategory);
            return Save();
        }

        public bool UpdatePostCategory(PostCategory postCategory)
        {
            _context.Update(postCategory);
            return Save();
        }

        public PostCategory? GetPostCategory(int postId, int categoryId)
        {
            return _context.PostCategories.FirstOrDefault(c => c.PostId == postId && c.CategoryId == categoryId);
        }

        public ICollection<PostCategory> GetPostCategoriesByCategoryId(int categoryId)
        {
            return _context.PostCategories.Where(c => c.CategoryId == categoryId).ToList();
        }

        public ICollection<PostCategory> GetPostCategoriesByPostId(int postId)
        {
            return _context.PostCategories.Where(c => c.PostId == postId).ToList();
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                // if saved > 0 then return true, else return false
                return saved > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (DBConcurrencyException)
            {
                return false;
            }
        }
    }
}