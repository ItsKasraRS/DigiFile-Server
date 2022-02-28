using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BackEnd.DataLayer.Entities.Product;

namespace BackEnd.Core.Interfaces
{
    public interface ICategoryService
    {
        
    }

    public interface IAdminCategoryService
    {
        Task<List<Category>> GetCategories();
        Task<List<Category>> GetSubCategories(int catId);
    }
}
