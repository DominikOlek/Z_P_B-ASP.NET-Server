using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiP.Services;
using System.ComponentModel.DataAnnotations;

namespace ApiP.Models
{
    public class PageResult<T>
    {
        public List<T> Result { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int PageNumber { get; set; }


        public PageResult(List<T> _result, int totalNumber, int pageSize, int pageNumber)
        {
            Result = _result;
            TotalItems = totalNumber;
            ItemsFrom = pageSize * (pageNumber-1)+1;
            PageNumber = pageNumber;
            ItemsTo = ItemsFrom + pageSize -1;
            if (ItemsTo > TotalItems) ItemsTo = TotalItems;
            TotalPages = (int)Math.Ceiling(totalNumber/(double)pageSize);
        }
    }
}

