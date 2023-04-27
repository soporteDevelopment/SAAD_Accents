using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class StockViewModel
    {

        public int idInventario { get; set; }

        public int? idSucursal { get; set; }

        public string Sucursal { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int? DiferenciasNegativas { get; set; }

        public int? DiferenciasPositivas { get; set; }

        public int? ArticulosInventarioAnterior { get; set; }

        public int? ArticulosInventarioActual { get; set; }

        public decimal? CostoInventarioAnterior { get; set; }

        public decimal? CostoInventarioActual { get; set; }

        public short? Estatus { get; set; }

    }
}