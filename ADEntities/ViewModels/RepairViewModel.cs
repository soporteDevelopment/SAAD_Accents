using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class RepairViewModel
    {
        public int idReparacion { get; set; }
        public string Numero { get; set; }
        public int idUsuario { get; set; }
        public string Destino { get; set; }
        public string Responsable { get; set; }
        public Nullable<System.DateTime> FechaSalida { get; set; }
        public Nullable<System.DateTime> FechaRegreso { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> Estatus { get; set; }
        public string sEstatus { get; set; }
        public string ColorEstatus { get; internal set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public List<DetailRepairViewModel> lDetalle { get; set; }
        public string Usuario { get; internal set; }
    }
}