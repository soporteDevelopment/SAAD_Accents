using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Filters
{
    public class SaleOnLineFilters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Status { get; set; }
        public string Customer { get; set; }
        public string User { get; set; }
    }
}