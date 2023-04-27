using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PreQuotationsDetailsViewModel
    {
        public int idPreCotizacionDetalle { get; set; }
        public int idPreCotizacion { get; set; }
        public int? idServicio { get; set; }
        public int? idEstatus { get; set; }
        public string NombreEstatus { get; set; }
        public string DescripcionEstatus { get; set; }
        public string NombreTipoServicio { get; set; }
        public string Descripcion { get; set; }
        public string Comentarios { get; set; }
        public int? Cantidad { get; set; }
        public string Imagen { get; set; }
        public string PDF { get; set; }
        public List<PreCotDetalleMedidasViewModel> measures { get; set; }
        public List<PreCotDetalleTipoTelaViewModel> fabrics { get; set; }
    }
}