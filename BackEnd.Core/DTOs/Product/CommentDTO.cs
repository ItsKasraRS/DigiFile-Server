using System;
using System.Collections.Generic;
using System.Text;

namespace BackEnd.Core.DTOs.Product
{
    public class ShowCommentsDTO
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string UserName { get; set; }

        public string UserAvatar { get; set; }
    }

    public class AddCommentDTO
    {
        public string Text { get; set; }
    }

    public class UserCommentsDTO
    {
        public string Text { get; set; }

        public string Status { get; set; }

        public DateTime CreateDate { get; set; }

        public string ProductTitle { get; set; }

        public long ProductId { get; set; }
    }
}
