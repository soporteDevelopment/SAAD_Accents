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
    
    public partial class tVenta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tVenta()
        {
            this.tDetalleVentas = new HashSet<tDetalleVenta>();
            this.tEntradas = new HashSet<tEntrada>();
            this.tFormaPagoes = new HashSet<tFormaPago>();
            this.tHistorialCreditoes = new HashSet<tHistorialCredito>();
            this.tMesaRegalos = new HashSet<tMesaRegalo>();
            this.tNotasCreditoes = new HashSet<tNotasCredito>();
            this.tSalidas = new HashSet<tSalida>();
            this.tVistas = new HashSet<tVista>();
        }
    
        public int idVenta { get; set; }
        public string Remision { get; set; }
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
        public Nullable<short> Estatus { get; set; }
        public Nullable<int> idVista { get; set; }
        public Nullable<short> IVA { get; set; }
        public Nullable<short> Factura { get; set; }
        public string Comentarios { get; set; }
        public string NumeroFactura { get; set; }
        public Nullable<byte> TipoCliente { get; set; }
        public Nullable<int> idDespacho { get; set; }
        public string Proyecto { get; set; }
        public Nullable<bool> PagadoUno { get; set; }
        public Nullable<int> idCotizacion { get; set; }
        public Nullable<short> TipoVenta { get; set; }
        public Nullable<bool> Omitir { get; set; }
        public Nullable<int> idVentasComisiones { get; set; }
        public Nullable<int> idVentasComisionesDespacho { get; set; }
        public Nullable<bool> PagadoOffice { get; set; }
        public Nullable<bool> PagadoDos { get; set; }
        public Nullable<decimal> ComisionGeneral { get; set; }
        public Nullable<decimal> ComisionArteriors { get; set; }
        public Nullable<decimal> ComisionTotal { get; set; }
        public Nullable<bool> Editar { get; set; }
        public Nullable<decimal> TotalNotaCredito { get; set; }
        public Nullable<int> OrigenMoneda { get; set; }
        public Nullable<int> idOrigen { get; set; }
    
        public virtual tClientesFisico tClientesFisico { get; set; }
        public virtual tClientesMorale tClientesMorale { get; set; }
        public virtual tCotizacion tCotizacion { get; set; }
        public virtual tDespacho tDespacho { get; set; }
        public virtual tDespacho tDespacho1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleVenta> tDetalleVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tEntrada> tEntradas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tFormaPago> tFormaPagoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tHistorialCredito> tHistorialCreditoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tMesaRegalo> tMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNotasCredito> tNotasCreditoes { get; set; }
        public virtual tOrigenVenta tOrigenVenta { get; set; }
        public virtual tReporteVentasComisione tReporteVentasComisione { get; set; }
        public virtual tReporteVentasComisionesDespacho tReporteVentasComisionesDespacho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tSalida> tSalidas { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVista> tVistas { get; set; }
    }
}
