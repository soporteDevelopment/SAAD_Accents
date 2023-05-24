using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PreCotDetalleProveedoresViewModel
    {
        public int idPreCotDetalleProveedores { get; set; }
        public int? idPreCotizacionDetalle { get; set; }
        public int? idTipoServicio { get; set; }
        public int? idProveedor { get; set; }
        public string TipoProveedor { get; set; }
        public string Proveedor { get; set; }
        public decimal? CostoFabricacion { get; set; }
        public decimal? CostoPublico { get; set; }
        public int? DiasFabricacion { get; set; }
        public string ComentariosProveedor { get; set; }
        public string ComentariosComprador { get; set; }
        public int? Enviado { get; set; }
        public int? Asignado { get; set; }
        public List<PreCotProveedoresTelasViewModel> TelasProveedores { get; set; }
    }
}