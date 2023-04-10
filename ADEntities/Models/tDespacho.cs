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
    
    public partial class tDespacho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tDespacho()
        {
            this.tBorradorVentas = new HashSet<tBorradorVenta>();
            this.tBorradorVentas1 = new HashSet<tBorradorVenta>();
            this.tBorradorVistas = new HashSet<tBorradorVista>();
            this.tBorradorVistas1 = new HashSet<tBorradorVista>();
            this.tCatalogoVistas = new HashSet<tCatalogoVista>();
            this.tCatalogoVistas1 = new HashSet<tCatalogoVista>();
            this.tCotizacions = new HashSet<tCotizacion>();
            this.tCotizacions1 = new HashSet<tCotizacion>();
            this.tMesaRegalos = new HashSet<tMesaRegalo>();
            this.tNotasCreditoes = new HashSet<tNotasCredito>();
            this.tPedidos = new HashSet<tPedido>();
            this.tReporteVentasComisionesDespachos = new HashSet<tReporteVentasComisionesDespacho>();
            this.tVentaMesaRegalos = new HashSet<tVentaMesaRegalo>();
            this.tVentaMesaRegalos1 = new HashSet<tVentaMesaRegalo>();
            this.tVentas = new HashSet<tVenta>();
            this.tVentas1 = new HashSet<tVenta>();
            this.tVistas = new HashSet<tVista>();
            this.tVistas1 = new HashSet<tVista>();
        }
    
        public int idDespacho { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Calle { get; set; }
        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string Colonia { get; set; }
        public Nullable<int> idMunicipio { get; set; }
        public Nullable<int> CP { get; set; }
        public string Correo { get; set; }
        public Nullable<decimal> Comision { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> idOrigen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVenta> tBorradorVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVenta> tBorradorVentas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVista> tBorradorVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVista> tBorradorVistas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCatalogoVista> tCatalogoVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCatalogoVista> tCatalogoVistas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCotizacion> tCotizacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCotizacion> tCotizacions1 { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tMunicipio tMunicipio { get; set; }
        public virtual tOrigenCliente tOrigenCliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tMesaRegalo> tMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNotasCredito> tNotasCreditoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPedido> tPedidos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tReporteVentasComisionesDespacho> tReporteVentasComisionesDespachos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVentaMesaRegalo> tVentaMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVentaMesaRegalo> tVentaMesaRegalos1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVenta> tVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVenta> tVentas1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVista> tVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVista> tVistas1 { get; set; }
    }
}
