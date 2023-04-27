using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class InformationStockViewModel
    {

        public decimal? AmountNegatives { get; set; }

        public decimal? AmountPositives { get; set; }

        public decimal? AmountOldStock { get; set; }

        public decimal? AmountNewStock { get; set; }

        public decimal? CostOldStock { get; set; }

        public decimal? CostNewStock { get; set; }

    }
}