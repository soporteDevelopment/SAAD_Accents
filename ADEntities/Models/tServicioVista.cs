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
    
    public partial class tServicioVista
    {
        public int idServicioVista { get; set; }
        public Nullable<int> idVista { get; set; }
        public Nullable<int> idServicio { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public Nullable<short> Estatus { get; set; }
        public string Comentarios { get; set; }
    
        public virtual tServicio tServicio { get; set; }
        public virtual tVista tVista { get; set; }
    }
}
