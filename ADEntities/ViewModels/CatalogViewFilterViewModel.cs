using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CatalogViewFilterViewModel
    /// </summary>
    public class CatalogViewFilterViewModel
    {
        /// <summary>
        /// Gets or sets the date since.
        /// </summary>
        /// <value>
        /// The date since.
        /// </value>
        public string DateSince { get; set; }
        /// <summary>
        /// Gets or sets the date until.
        /// </summary>
        /// <value>
        /// The date until.
        /// </value>
        public string DateUntil { get; set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Number { get; set; }
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public string Customer { get; set; }
        /// <summary>
        /// Gets or sets the catalog code.
        /// </summary>
        /// <value>
        /// The catalog code.
        /// </value>
        public string CatalogCode { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; set; }
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public int Page { get; set; }
        /// <summary>
        /// The page size
        /// </summary>
        public int PageSize { get; set; }
    }
}