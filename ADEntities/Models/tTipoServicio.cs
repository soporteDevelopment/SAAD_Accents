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
    
    public partial class tTipoServicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tTipoServicio()
        {
            this.tProveedores = new HashSet<tProveedore>();
        }
    
        public int idTipoServicio { get; set; }
        public string Descripcion { get; set; }
        public decimal FactorUtilidad { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tProveedore> tProveedores { get; set; }
    }
}