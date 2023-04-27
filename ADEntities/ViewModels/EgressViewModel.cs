using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class EgressViewModel
    {
        public int idSalida { get; set; }
        public DateTime? Fecha { get; set; }
        public int? RecibidaPor { get; set; }
        public string Recibida { get; set; }
        public string RecibidaOtro { get; set; }
        public int? Tipo { get; set; }
        public int? idEntrada { get; set; }
        public int? idVenta { get; set; }
        public decimal? Cantidad { get; set; }
        public string Remision { get; set; }
        public string Comentarios { get; set; }
        public int? fkSalida { get; set; }
        public int? Estatus { get; set; }
        public int? CreadoPor { get; set; }
        public DateTime? Creado { get; set; }
        public int? ModificadoPor { get; set; }
        public DateTime? Modificado { get; set; }
        public List<InternalEgressViewModel> InternalEgresses { get; set; }
    }
}