using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class PromotionViewModel
    {
        public int idPromocion { get; set; }
        public int idTipoPromocion { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Costo { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public TypePromotionViewModel TipoPromocion { get; set; }
        public List<DetailPromotionViewModel> DetallePromociones { get; set; }
    }
}