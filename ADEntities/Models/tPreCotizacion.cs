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
    
    public partial class tPreCotizacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tPreCotizacion()
        {
            this.tHistEstatusPreCots = new HashSet<tHistEstatusPreCot>();
            this.tHistEstatusServicios = new HashSet<tHistEstatusServicio>();
            this.tPreCotizacionDetalles = new HashSet<tPreCotizacionDetalle>();
            this.tPreCotizacionDetalles1 = new HashSet<tPreCotizacionDetalle>();
            this.tRelCotPrecots = new HashSet<tRelCotPrecot>();
        }
    
        public int idPreCotizacion { get; set; }
        public Nullable<int> idUsuario1 { get; set; }
        public Nullable<int> idUsuario2 { get; set; }
        public Nullable<int> idClienteFisico { get; set; }
        public Nullable<int> idClienteMoral { get; set; }
        public Nullable<int> idDespachoReferencia { get; set; }
        public Nullable<int> idSucursal { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> CantidadProductos { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> idDespacho { get; set; }
        public Nullable<byte> TipoCliente { get; set; }
        public string Proyecto { get; set; }
        public Nullable<short> idEstatus { get; set; }
        public string Comentarios { get; set; }
        public string Numero { get; set; }
        public Nullable<int> idEvento { get; set; }
    
        public virtual tClientesFisico tClientesFisico { get; set; }
        public virtual tClientesMorale tClientesMorale { get; set; }
        public virtual tDespacho tDespacho { get; set; }
        public virtual tDespacho tDespacho1 { get; set; }
        public virtual tEstatusPrecotizacion tEstatusPrecotizacion { get; set; }
        public virtual tEvento tEvento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tHistEstatusPreCot> tHistEstatusPreCots { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tHistEstatusServicio> tHistEstatusServicios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPreCotizacionDetalle> tPreCotizacionDetalles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPreCotizacionDetalle> tPreCotizacionDetalles1 { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRelCotPrecot> tRelCotPrecots { get; set; }
    }
}