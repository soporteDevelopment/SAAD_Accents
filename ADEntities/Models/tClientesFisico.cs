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
    
    public partial class tClientesFisico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tClientesFisico()
        {
            this.tBorradorVentas = new HashSet<tBorradorVenta>();
            this.tBorradorVistas = new HashSet<tBorradorVista>();
            this.tCatalogoVistas = new HashSet<tCatalogoVista>();
            this.tCotizacions = new HashSet<tCotizacion>();
            this.tMesaRegalos = new HashSet<tMesaRegalo>();
            this.tMesaRegalos1 = new HashSet<tMesaRegalo>();
            this.tNotasCreditoes = new HashSet<tNotasCredito>();
            this.tPedidos = new HashSet<tPedido>();
            this.tVentaMesaRegalos = new HashSet<tVentaMesaRegalo>();
            this.tVentas = new HashSet<tVenta>();
            this.tVistas = new HashSet<tVista>();
            this.tPreCotizacions = new HashSet<tPreCotizacion>();
        }
    
        public int idCliente { get; set; }
        public string Nombre { get; set; }
        public string Calle { get; set; }
        public string NumExt { get; set; }
        public string NumInt { get; set; }
        public string Colonia { get; set; }
        public Nullable<int> idMunicipio { get; set; }
        public Nullable<int> CP { get; set; }
        public string TelefonoCelular { get; set; }
        public string Telefono { get; set; }
        public Nullable<System.DateTime> FechaAlta { get; set; }
        public Nullable<System.DateTime> FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Correo { get; set; }
        public string NumeroIFE { get; set; }
        public string RFC { get; set; }
        public Nullable<short> Credito { get; set; }
        public Nullable<int> Plazo { get; set; }
        public Nullable<decimal> LimiteCredito { get; set; }
        public string NombreIntermediario { get; set; }
        public string TelefonoIntermediario { get; set; }
        public string Apellidos { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> idOrigen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVenta> tBorradorVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBorradorVista> tBorradorVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCatalogoVista> tCatalogoVistas { get; set; }
        public virtual tUsuario tUsuario { get; set; }
        public virtual tMunicipio tMunicipio { get; set; }
        public virtual tOrigenCliente tOrigenCliente { get; set; }
        public virtual tOrigenCliente tOrigenCliente1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tCotizacion> tCotizacions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tMesaRegalo> tMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tMesaRegalo> tMesaRegalos1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tNotasCredito> tNotasCreditoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPedido> tPedidos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVentaMesaRegalo> tVentaMesaRegalos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVenta> tVentas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tVista> tVistas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPreCotizacion> tPreCotizacions { get; set; }
    }
}
