using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Core.DTOs;
using BackEnd.Core.DTOs.Product;
using BackEnd.Core.Interfaces;
using Backend.Core.Utilities.Extensions;
using BackEnd.Core.Utilities.Extensions;
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

        // Site Services

        #region Get Latest products - site

        public async Task<List<GetHomeProducts>> GetLatestProducts()
        {
            return await _context.Products.Where(p => !p.IsDelete).OrderByDescending(p => p.ReleaseDate).Take(8).Select(p =>
                new GetHomeProducts()
                {
                    Title = p.Title,
                    Price = p.Price,
                    Image = p.Image
                }).ToListAsync();
        }

        #endregion

        #region Get popular products - site
        
        public async Task<List<GetHomeProducts>> GetPopularProducts()
        {
            return await _context.Products.Where(p => !p.IsDelete).OrderByDescending(p => p.Like).Take(8).Select(p =>
                new GetHomeProducts()
                {
                    Title = p.Title,
                    Price = p.Price,
                    Image = p.Image
                }).ToListAsync();
        }

        #endregion

        #region Filter products - site

        public async Task<FilterProductsDTO> FilterProducts(FilterProductsDTO filter)
        {
            IQueryable<Product> product = _context.Products;
            if (!string.IsNullOrEmpty(filter.Title))
            {
                product = product.Where(p => p.Title.Contains(filter.Title));
            }

            if (filter.Categories != null)
            {
                product = product.Where(p => p.CategoryId == filter.Categories || p.SubCategoryId == filter.Categories);
            }

            switch (filter.SortBy)
            {
                case ProductSort.Latest:
                    product = product.OrderByDescending(p => p.ReleaseDate);
                    break;

                case ProductSort.Popular:
                    product = product.OrderByDescending(p => p.Like);
                    break;

                case ProductSort.PriceAsc:
                    product = product.OrderBy(p => p.Price);
                    break;

                case ProductSort.PriceDesc:
                    product = product.OrderByDescending(p => p.Price);
                    break;
            }

            filter.Products = await product.Select(p=>new ProductItemDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Image = p.Image
            }).ToListAsync();
            var count = (int)Math.Ceiling(product.Count() / (double)filter.TakeEntity);
            var pager = Pager.Build(count, filter.PageId, filter.TakeEntity);
            var list = await product.Paging(pager).Select(p=>new ProductItemDTO()
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Image = p.Image
            }).ToListAsync();
            return filter.SetProduct(list).SetPaging(pager);
        }

        public async Task<ProductDetailsDTO> GetProductDetails(long id)
        {
            var detail = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            var product = await _context.Products.Where(p => p.Id == id && !p.IsDelete).Select(p=>new ProductDetailsDTO
            {
                Id=p.Id,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Image = p.Image,
                Like = p.Like,
                Price = p.Price,
                ProductInfos = p.ProductInfos.Select(p=> new ProductInfoDTO { InfoTitle = p.InfoTitle , InfoValue = p.InfoValue }).ToList(),
                ProductPictures = null,
                ReleaseDate = p.ReleaseDate,
                SourceFile = p.SourceFile,
                SubCategoryId = p.SubCategoryId,
                Title = p.Title
            }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<Product> GetProductForUserOrder(long productId)
        {
            return await _context.Products.SingleOrDefaultAsync(p => !p.IsDelete && p.Id == productId);
        }

        public async Task<List<ProductGallery>> GetProductGallery(long productId)
        {
            return await _context.ProductGalleries.Where(pg => pg.ProductId == productId).ToListAsync();
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

        // Admin Services

        #region Get products - admin

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

        #endregion

        #region Add product - admin

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

        #endregion

        #region Get single product - admin

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

        #endregion

        #region Edit product - admin

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
                if (model.Image == null)
                {
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

        #endregion

        #region Remove product - admin

        public async Task RemoveProduct(long id)
        {
            var product = _context.Products.Find(id);
            product.IsDelete = true;
            _context.Update(product);
            await _context.SaveChangesAsync();
        }


        #endregion
    }
}
