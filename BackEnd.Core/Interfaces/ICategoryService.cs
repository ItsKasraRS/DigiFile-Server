using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Category;
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
        Task<List<Category>> ListAllSubCategories();
        Task<Category> AddCategory(AddCategoryDTO model);
        Task<Category> AddSubCategory(AddSubCategoryDTO model);
        Task<Category> GetCategory(int catId);
        Task EditCategory(Category model);
        Task RemoveCategory(int catId);
    }
}
