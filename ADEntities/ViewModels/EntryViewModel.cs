using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class EntryViewModel
    {
        public int idEntrada { get; set; }
        public DateTime? Fecha { get; set; }
        public int? EntregadaPor { get; set; }
        public string Entregada { get; set; }
        public string EntregadaOtro { get; set; }
        public int? Tipo { get; set; }
        public int? idVenta { get; set; }
        public decimal? Cantidad { get; set; }
        public string Comentarios { get; set; }
        public string Remision { get; set; }
        public int? Estatus { get; set; }
        public int? CreadoPor { get; set; }
        public DateTime? Creado { get; set; }
        public int? ModificadoPor { get; set; }
        public DateTime? Modificado { get; set; }
        public List<PaymentEntryViewModel> Payments { get; set; }
    }
}