using System;
using System.Collections.Generic;
using System.Text;
using BackEnd.DataLayer.Entities.Product;
using BackEnd.DataLayer.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.DataLayer.Context
{
    public class SiteContext : DbContext
    {
        public SiteContext(DbContextOptions<SiteContext> options) : base (options) { }

        #region User

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion

        #region Product

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        #endregion

    }
}
