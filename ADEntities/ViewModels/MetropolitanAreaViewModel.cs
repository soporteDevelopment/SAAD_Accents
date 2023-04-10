using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class MetropolitanAreaViewModel
    {
        public int idAreaMetropolitana { get; set; }
        public int idMunicipio { get; set; } 
        public TownViewModel Municipio { get; set; }
    }
}