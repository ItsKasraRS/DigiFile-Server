using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Core.DTOs.Product
{
    public class GetHomeProducts
    {
        public string Image { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
