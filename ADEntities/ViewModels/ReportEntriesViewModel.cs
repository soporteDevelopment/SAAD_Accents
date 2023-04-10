using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class ReportEntriesViewModel
    {
        public int idReporteCaja { get; set; }
        public DateTime? Fecha { get; set; }
        public string Comentarios { get; set; }
        public decimal? CantidadIngreso { get; set; }
        public decimal? CantidadEgreso { get; set; }
        public bool? Revisado { get; set; }
        public int? CreadoPor { get; set; }
        public DateTime? Creado { get; set; }
        public int? ModificadoPor { get; set; }
        public DateTime? Modificado { get; set; }
        public List<EntryViewModel> Ingresos { get; set; }
        public List<EgressViewModel> Egresos { get; set; }
    }
}