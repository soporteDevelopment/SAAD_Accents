using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class ActivityViewModel
    {
        public int idActividad { get; set; }
        public Nullable<short> TipoReporte { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Comentarios { get; set; }
        public Nullable<short> Estatus { get; set; }
    }
}