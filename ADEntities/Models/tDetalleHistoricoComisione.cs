//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADEntities.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tDetalleHistoricoComisione
    {
        public int idDetalleHistoricoComisones { get; set; }
        public Nullable<int> idHistoricoComisiones { get; set; }
        public Nullable<decimal> LimiteInferior { get; set; }
        public Nullable<decimal> LimiteSuperior { get; set; }
        public Nullable<decimal> SueldoMensual { get; set; }
        public Nullable<decimal> PorcentajeComision { get; set; }
        public Nullable<decimal> BonoUno { get; set; }
        public Nullable<decimal> BonoDos { get; set; }
        public Nullable<decimal> BonoTres { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<decimal> SueldoComision { get; set; }
    
        public virtual tHistoricoComisione tHistoricoComisione { get; set; }
    }
}
