using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PreCotDetalleMedidasViewModel
    {
        public int idPreCotDetalleMedidas { get; set; }
        public int? idPreCotizacionDetalle { get; set; }
        public int? idServicio { get; set; }
        public int? idTipoMedida { get; set; }
        public string NombreTipoMedida { get; set; }
        public decimal? Valor { get; set; }
    }
}