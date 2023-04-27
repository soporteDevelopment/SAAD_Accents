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
    
    public partial class tMesaRegalo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tMesaRegalo()
        {
            this.tDetalleMesaRegalos = new HashSet<tDetalleMesaRegalo>();
            this.tVentaMesaRegalos = new HashSet<tVentaMesaRegalo>();
        }
    
        public int idMesaRegalo { get; set; }
        public Nullable<int> idNovio { get; set; }
        public Nullable<int> idNovia { get; set; }
        public Nullable<System.DateTime> FechaBoda { get; set; }
        public string LugarBoda { get; set; }
        public string HoraBoda { get; set; }
        public Nullable<int> idSucursal { get; set; }
        public Nullable<int> idDespachoReferencia { get; set; }
        public Nullable<int> CantidadProductos { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> Descuento { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<short> Estatus { get; set; }
        public Nullable<int> idVenta { get; set; }
        public Nullable<int> idNotaCredito { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> Notificar { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> idUsuario1 { get; set; }
        public Nullable<int> idUsuario2 { get; set; }
        public string Remision { get; set; }
        public Nullable<short> IVA { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    
        public virtual tClientesFisico tClientesFisico { get; set; }
        public virtual tClientesFisico tClientesFisico1 { get; set; }
        public virtual tDespacho tDespacho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDetalleMesaRegalo> tDetalleMesaRegalos { get; set; }
        public virtual tNotasCredito tNotasCredito { get; set; }
        public virtual tSucursale tSucursale { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tUsuario tUsuario1 { get; set; }
        public virtual tVenta tVenta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVentaMesaRegalo> tVentaMesaRegalos { get; set; }
    }
}
