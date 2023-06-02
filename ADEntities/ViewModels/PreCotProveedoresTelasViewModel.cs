using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PreCotProveedoresTelasViewModel
    {
        public int idPreCotProveedoresTelas { get; set; }
        public int? idPreCotDetalleProveedores { get; set; }
        public int? idServicio { get; set; }
        public int? idProveedor { get; set; }
        public int? idTextiles { get; set; }
        public decimal? ValorMts { get; set; }
        public decimal? CostoPorMts { get; set; }
    }
}