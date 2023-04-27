using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    ///  CatalogBrandViewModel
    /// </summary>
    public class CatalogBrandViewModel
    {
        public int idCatalogBrand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int idProvider { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool Active { get; set; }
    }
}