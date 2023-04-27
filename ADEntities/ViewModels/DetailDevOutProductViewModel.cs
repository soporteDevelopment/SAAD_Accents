using System;

namespace ADEntities.ViewModels
{
    public class DetailDevOutProductViewModel
    {
        public int idDetalleDevoluciones { get; set; }
        public int? idDetalleVista { get; set; }        
        public decimal? Devolucion { get; set; }
        public decimal? Venta { get; set; }
        public string Remision { get; set; }
        public DateTime? Fecha { get; set; }
        public int? idVerificador { get; set; }
        public string Verificador { get; set; }
    }
}
