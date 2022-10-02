using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Category;
using BackEnd.Core.Interfaces;
using BackEnd.DataLayer.Context;
using BackEnd.DataLayer.Entities.Product;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Core.Services
{
    public class CategoryService : ICategoryService
    {
        #region constructor

        private SiteContext _context;

        public CategoryService(SiteContext context)
        {
            _context = context;
        }

        #endregion


        public async Task<List<Category>> GetCategoriesForHeader()
        {
            return await _context.Categories.Where(c => !c.IsDelete).ToListAsync();
        }
    }

    public class AdminCategoryService : IAdminCategoryService
    {
        #region constructor

        private SiteContext _context;

        public AdminCategoryService(SiteContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.Where(c => c.ParentId == null && !c.IsDelete).ToListAsync();
        }

        public async Task<List<Category>> GetSubCategories(int catId)
        {
            return await _context.Categories.Where(c => c.ParentId == catId && !c.IsDelete).ToListAsync();
        }

        public async Task<List<Category>> ListAllSubCategories()
        {
            return await _context.Categories.Where(c => c.ParentId != null && !c.IsDelete).ToListAsync();
        }

        public async Task<Category> AddCategory(AddCategoryDTO model)
        {
            var category = new Category()
            {
                Title = model.Title,
                ParentId = model.ParentId,
                IsDelete = false
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> AddSubCategory(AddSubCategoryDTO model)
        {
            var category = new Category()
            {
                Title = model.Title,
                ParentId = model.ParentId,
                IsDelete = false
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> GetCategory(int catId)
        {
            return await _context.Categories.FindAsync(catId);
        }

        public async Task EditCategory(Category model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCategory(int catId)
        {
            var category = await GetCategory(catId);
            category.IsDelete = true;
            await EditCategory(category);
        }
    }
}
