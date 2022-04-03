using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.Product
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public bool IsDelete { get; set; }

        #region Relation

        [ForeignKey("ParentId")]
        public List<Category> Categories { get; set; }

        [InverseProperty("Category")]
        public List<Product> Products { get; set; }

        [InverseProperty("SubCategory")]
        public List<Product> SubProducts { get; set; }

        #endregion
    }
}
