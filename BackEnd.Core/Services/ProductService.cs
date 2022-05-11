using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using Backend.Core.Utilities.Extensions;
using BackEnd.DataLayer.Context;
using BackEnd.DataLayer.Entities.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Core.Services
{
    public class ProductService : IProductService
    {
        #region constructor

        private SiteContext _context;
        private ICategoryService _categoryService;

        public ProductService(SiteContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        #endregion
    }

    public class AdminProductService : IAdminProductService
    {
        #region constructor

        private SiteContext _context;
        private ICategoryService _categoryService;

        public AdminProductService(SiteContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        #endregion

        public async Task<AdminFilterProducts> GetProductsForAdmin(int pageId = 1, string filterTitle = "")
        {
            IQueryable<Product> product = _context.Products.Include(p => p.ProductInfos).Where(p => !p.IsDelete);
            if (!String.IsNullOrEmpty(filterTitle))
            {
                product = product.Where(p => p.Title.Contains(filterTitle));
            }

            int take = 6;
            int skip = (pageId - 1) * take;
            AdminFilterProducts list = new AdminFilterProducts();
            list.CurrentPage = pageId;
            list.PageCount = (int)Math.Ceiling(product.Count() / (double)take);
            list.Products = await product.OrderByDescending(p => p.ReleaseDate).Skip(skip).Take(take).ToListAsync();
            return list;
        }

        public async Task AddProductForAdmin(AddProductDTO model, IFormFile sourceFile)
        {
            var product = new Product()
            {
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                ReleaseDate = DateTime.Now,
                Like = 0,
                CategoryId = model.Category,
                SubCategoryId = model.SubCategory,
                Image = "no-image.jpg",
                SourceFile = ""
            };
            var imagePath = "";
            if (sourceFile != null)
            {
                product.SourceFile = Guid.NewGuid().ToString("N") + Path.GetExtension(sourceFile.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product/file/", product.SourceFile);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    sourceFile.CopyTo(stream);
                }
            }
            if (!string.IsNullOrEmpty(model.SelectedImage))
            {
                var imageFile = ImageUploaderExtension.Base64ToImage(model.SelectedImage);
                var imageName = Guid.NewGuid().ToString("N") + ".jpg";
                imageFile.AddImageToServer(imageName, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product/thumbnail/"));
                product.Image = imageName;
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<EditProductDTO> GetProductForAdmin(long id)
        {
            var product = await _context.Products.FindAsync(id);
            var model = new EditProductDTO()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Category = product.CategoryId,
                SubCategory = product.SubCategoryId,
                Image = product.Image,
                ReleaseDate = product.ReleaseDate,
                SourceFile = product.SourceFile
            };
            return model;
        }

        public async Task EditProduct(EditProductDTO model)
        {
            var product = _context.Products.Find(model.Id);
            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.Category;
            product.SubCategoryId = model.SubCategory;
            product.ReleaseDate = model.ReleaseDate;
            product.Image = model.Image;
            product.SourceFile = model.SourceFile;

            var imagePath = "";
            if (model.SelectedSourceFile != null)
            {
                if (model.Image == null) {
                    product.SourceFile = Guid.NewGuid().ToString("N") + Path.GetExtension(model.SelectedSourceFile.FileName);
                }

                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product/file/",
                    product.SourceFile);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    model.SelectedSourceFile.CopyTo(stream);
                }
            }

            if (!string.IsNullOrEmpty(model.SelectedImage))
            {
                var imageName = "";
                var imageFile = ImageUploaderExtension.Base64ToImage(model.SelectedImage);
                if (model.Image == "no-image.jpg")
                {
                    imageName = Guid.NewGuid().ToString("N") + ".jpg";
                }
                else
                {
                    imageName = model.Image;
                }
                imageFile.AddImageToServer(imageName, Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/product/thumbnail/"));
                product.Image = imageName;
            }

            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveProduct(long id)
        {
            var product = _context.Products.Find(id);
            product.IsDelete = true;
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
