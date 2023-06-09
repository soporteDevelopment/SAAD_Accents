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
    
    public partial class tDetalleRecepcion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tDetalleRecepcion()
        {
            this.tHistoricoRecepcions = new HashSet<tHistoricoRecepcion>();
        }
    
        public int idDetalleRecepcion { get; set; }
        public Nullable<int> idRecepcion { get; set; }
        public Nullable<int> idProducto { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public Nullable<decimal> Costo { get; set; }
        public Nullable<int> Estatus { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public Nullable<decimal> CantidadRecibida { get; set; }
    
        public virtual tUsuario tUsuario { get; set; }
        public virtual tProducto tProducto { get; set; }
        public virtual tRecepcione tRecepcione { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tHistoricoRecepcion> tHistoricoRecepcions { get; set; }
    }
}
