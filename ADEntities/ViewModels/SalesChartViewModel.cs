using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class SalesChartViewModel
    {
        public int VendedorId { get; set; }
        public string Vendedor { get; set; }

        public decimal VentasMesPasado { get; set; }

        public decimal VentasMesActual { get; set; }
    }
}