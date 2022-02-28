using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return await _context.Categories.Where(c => c.ParentId == null).ToListAsync();
        }

        public async Task<List<Category>> GetSubCategories(int catId)
        {
            return await _context.Categories.Where(c => c.ParentId == catId).ToListAsync();
        }
    }
}
