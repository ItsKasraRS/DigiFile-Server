using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Core.DTOs.Category
{
    public class AddCategoryDTO
    {
        public int? ParentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }

    public class AddSubCategoryDTO
    {
        [Required]
        public int ParentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
