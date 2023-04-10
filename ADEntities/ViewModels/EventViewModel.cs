using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class EventViewModel
    {
        public int idEvento { get; set; }
        public string Nombre { get; set; }
        public System.DateTime Fecha { get; set; }
        public bool? TodoDia { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string Lugar { get; set; }
        public string Color { get; set; }
        public string Fondo { get; set; }
        public string Fuente { get; set; }
        public Nullable<System.DateTime> FechaRecordatorio { get; set; }
        public string HoraRecordatorio { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public Nullable<bool> Cancelado { get; set; }
        public List<int> SelectedUsers { get; internal set; }
        public string CreadoPor { get; set; }
        public int? Tipo { get; internal set; }
    }
}