using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// Distribution of products
    /// </summary>
    public class DistributionViewModel
    {
        /// <summary>
        /// Gets or sets the identifier branch.
        /// </summary>
        /// <value>
        /// The identifier branch.
        /// </value>
        public int? idBranch { get; set; }
        /// <summary>
        /// Gets or sets the stock.
        /// </summary>
        /// <value>
        /// The stock.
        /// </value>
        public decimal Stock { get; set; }
    }
}