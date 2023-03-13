using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Core.DTOs.User
{
    public class ShowUserOrdersDTO
    {
        public long OrderId { get; set; }

        public DateTime CreateDate { get; set; }

        public decimal Sum { get; set; }

        public string IsPaid { get; set; }

        public List<OrderProductsDTO> Products { get; set; }
    }

    public class OrderProductsDTO
    {
        public long Id { get; set; }

        public string Title { get; set; }
    }
}
