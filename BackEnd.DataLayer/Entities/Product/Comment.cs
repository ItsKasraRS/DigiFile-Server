using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.Product
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ProductId { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public long? ParentId { get; set; }

        public bool IsDelete { get; set; }

        #region Refrences

        [ForeignKey("UserId")]
        public User.User User { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("ParentId")]
        public Comment ParentComment { get; set; }

        #endregion
    }
}
