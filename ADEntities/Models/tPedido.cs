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
    
    public partial class tPedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tPedido()
        {
            this.tDetallePedidoes = new HashSet<tDetallePedido>();
        }
    
        public int idPedido { get; set; }
        public Nullable<int> idUsuario1 { get; set; }
        public Nullable<int> idUsuario2 { get; set; }
        public Nullable<int> idClienteFisico { get; set; }
        public Nullable<int> idClienteMoral { get; set; }
        public Nullable<int> idDespacho { get; set; }
        public Nullable<int> idSucursal { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> CantidadProductos { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<short> IVA { get; set; }
    
        public virtual tClientesFisico tClientesFisico { get; set; }
        public virtual tClientesMorale tClientesMorale { get; set; }
        public virtual tDespacho tDespacho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetallePedido> tDetallePedidoes { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
    }
}
