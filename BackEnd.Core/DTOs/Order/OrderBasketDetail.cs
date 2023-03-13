using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Core.DTOs.Order
{
    public class OrderBasketDetail
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }
    }

    public class OrderCheckoutDTO
    {
        public decimal Price { get; set; }


    }
}
