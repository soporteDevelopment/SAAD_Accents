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
    
    public partial class tPerfilAccione
    {
        public int idPerfil { get; set; }
        public int idAccion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
    
        public virtual tAccione tAccione { get; set; }
        public virtual tPerfile tPerfile { get; set; }
    }
}
