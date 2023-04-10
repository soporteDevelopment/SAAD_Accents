using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class CommissionsSalesViewModel
    {
        public int idVentasComisiones { get; set; }
        public int idVendedor { get; set; }
        public int FormaPago { get; set; }
        public string sFormaPago { get; set; }
        public string NumCheque { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime FechaPago { get; set; }
        public string sConcepto { get; set; }
        public string Detalle { get; set; }
    }
}