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
    
    public partial class tCatalogoDetalleVista
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tCatalogoDetalleVista()
        {
            this.tCatalogoDetalleDevolucions = new HashSet<tCatalogoDetalleDevolucion>();
        }
    
        public int idDetalleVista { get; set; }
        public int idVista { get; set; }
        public int idCatalogo { get; set; }
        public decimal Precio { get; set; }
        public decimal Cantidad { get; set; }
        public int idSucursal { get; set; }
        public Nullable<decimal> Devolucion { get; set; }
        public string Comentarios { get; set; }
        public int Estatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCatalogoDetalleDevolucion> tCatalogoDetalleDevolucions { get; set; }
        public virtual tCatalogo tCatalogo { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tCatalogoVista tCatalogoVista { get; set; }
    }
}
