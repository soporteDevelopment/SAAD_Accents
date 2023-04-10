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
    
    public partial class tServicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tServicio()
        {
            this.tBorradorServicioVistas = new HashSet<tBorradorServicioVista>();
            this.tDetalleCreditoes = new HashSet<tDetalleCredito>();
            this.tDetalleMesaRegalos = new HashSet<tDetalleMesaRegalo>();
            this.tDetalleVentaEnLineas = new HashSet<tDetalleVentaEnLinea>();
            this.tRespaldoDetalleVentaEnLineas = new HashSet<tRespaldoDetalleVentaEnLinea>();
            this.tServicioVistas = new HashSet<tServicioVista>();
        }
    
        public int idServicio { get; set; }
        public string Descripcion { get; set; }
        public Nullable<short> Instalacion { get; set; }
        public Nullable<bool> AplicarDescuento { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorServicioVista> tBorradorServicioVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleCredito> tDetalleCreditoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleMesaRegalo> tDetalleMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleVentaEnLinea> tDetalleVentaEnLineas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tRespaldoDetalleVentaEnLinea> tRespaldoDetalleVentaEnLineas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tServicioVista> tServicioVistas { get; set; }
    }
}
