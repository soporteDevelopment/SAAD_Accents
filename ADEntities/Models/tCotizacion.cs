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
    
    public partial class tCotizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tCotizacion()
        {
            this.tCotizacionDetalles = new HashSet<tCotizacionDetalle>();
            this.tDetalleVentas = new HashSet<tDetalleVenta>();
            this.tVentas = new HashSet<tVenta>();
            this.tRelCotPrecots = new HashSet<tRelCotPrecot>();
        }
    
        public int idCotizacion { get; set; }
        public Nullable<int> idUsuario1 { get; set; }
        public Nullable<int> idUsuario2 { get; set; }
        public Nullable<int> idClienteFisico { get; set; }
        public Nullable<int> idClienteMoral { get; set; }
        public Nullable<int> idDespachoReferencia { get; set; }
        public Nullable<int> idSucursal { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> CantidadProductos { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<short> IVA { get; set; }
        public Nullable<int> idDespacho { get; set; }
        public Nullable<byte> TipoCliente { get; set; }
        public string Proyecto { get; set; }
        public Nullable<short> Estatus { get; set; }
        public string Comentarios { get; set; }
        public Nullable<bool> Dolar { get; set; }
        public Nullable<int> idVista { get; set; }
        public string Numero { get; set; }
        public Nullable<int> idTipoCotizacion { get; set; }
        public Nullable<int> idOrigen { get; set; }
        public decimal IVATasa { get; set; }
    
        public virtual tClientesFisico tClientesFisico { get; set; }
        public virtual tClientesMorale tClientesMorale { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCotizacionDetalle> tCotizacionDetalles { get; set; }
        public virtual tDespacho tDespacho { get; set; }
        public virtual tDespacho tDespacho1 { get; set; }
        public virtual tOrigenVenta tOrigenVenta { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tTipoCotizacion tTipoCotizacion { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
        public virtual tVista tVista { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleVenta> tDetalleVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVenta> tVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRelCotPrecot> tRelCotPrecots { get; set; }
    }
}
