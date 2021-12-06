using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEnd.DataLayer.Entities.Product
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public int OrderSum { get; set; }
        public bool IsFinally { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        #region Refrence

        [ForeignKey("UserId")]
        public User.User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        #endregion
    }

    public class OrderDetail
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long OrderId { get; set; }
        [Required]
        public long ProductId { get; set; }
        [Required]
        public int Price { get; set; }

        #region Refrence

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        #endregion
    }
}
