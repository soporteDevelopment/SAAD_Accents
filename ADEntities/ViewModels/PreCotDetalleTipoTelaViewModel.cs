using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PreCotDetalleTipoTelaViewModel
    {
        public int idPreCotDetalleTipoTela { get; set; }
        public int? idPreCotizacionDetalle { get; set; }
        public int? idServicio { get; set; }
        public int? idTextiles { get; set; }
        public string NombreTextiles { get; set; }
        public decimal? CostoPorMts { get; set; }
        public decimal? ValorMts { get; set; }
        public decimal? CostoTotal { get; set; }
    }
}