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
    
    public partial class tPerfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tPerfile()
        {
            this.tPerfilAcciones = new HashSet<tPerfilAccione>();
            this.tUsuarios = new HashSet<tUsuario>();
        }
    
        public int idPerfil { get; set; }
        public string Perfil { get; set; }
        public Nullable<short> Estatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPerfilAccione> tPerfilAcciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tUsuario> tUsuarios { get; set; }
    }
}
