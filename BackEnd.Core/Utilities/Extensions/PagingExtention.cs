using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackEnd.Core.DTOs;

namespace BackEnd.Core.Utilities.Extensions
{
    public static class PagingExtension
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> model, BasePaging pager)
        {
            return model.Skip(pager.SkipEntity).Take(pager.TakeEntity);
        }
    }
}
