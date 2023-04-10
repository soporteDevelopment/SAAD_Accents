using System;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// View model of CreditNote
    /// </summary>
    public class CreditNoteViewModel
    {
        /// <summary>
        /// Gets or sets the identifier nota credito.
        /// </summary>
        /// <value>
        /// The identifier nota credito.
        /// </value>
        public int idNotaCredito { get; set; }
        /// <summary>
        /// Gets or sets the identifier tipo nota credito.
        /// </summary>
        /// <value>
        /// The identifier tipo nota credito.
        /// </value>
        public int? idTipoNotaCredito { get; set; }
        /// <summary>
        /// Gets or sets the tipo nota credito.
        /// </summary>
        /// <value>
        /// The tipo nota credito.
        /// </value>
        public CreditNoteTypeViewModel TipoNotaCredito { get; set; }
        /// <summary>
        /// Gets or sets the identifier venta.
        /// </summary>
        /// <value>
        /// The identifier venta.
        /// </value>
        public int? idVenta { get; set; }
        /// <summary>
        /// Gets or sets the remision credito.
        /// </summary>
        /// <value>
        /// The remision credito.
        /// </value>
        public string RemisionCredito {get; set;}
        /// <summary>
        /// Gets or sets the remision venta.
        /// </summary>
        /// <value>
        /// The remision venta.
        /// </value>
        public string RemisionVenta { get; set; }
        /// <summary>
        /// Gets or sets the cliente.
        /// </summary>
        /// <value>
        /// The cliente.
        /// </value>
        public string Cliente { get; set; }
        /// <summary>
        /// Gets or sets the cliente venta.
        /// </summary>
        /// <value>
        /// The cliente venta.
        /// </value>
        public string ClienteVenta { get; set; }
        /// <summary>
        /// Gets or sets the identifier vendedor.
        /// </summary>
        /// <value>
        /// The identifier vendedor.
        /// </value>
        public int? idVendedor { get; set; }
        /// <summary>
        /// Gets or sets the vendedor.
        /// </summary>
        /// <value>
        /// The vendedor.
        /// </value>
        public string Vendedor { get; set; }
        /// <summary>
        /// Gets or sets the cantidad.
        /// </summary>
        /// <value>
        /// The cantidad.
        /// </value>
        public decimal? Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        public DateTime? Fecha { get; set; }
        /// <summary>
        /// Gets or sets the fecha vigencia.
        /// </summary>
        /// <value>
        /// The fecha vigencia.
        /// </value>
        public DateTime? FechaVigencia { get; set; }
        /// <summary>
        /// Gets or sets the comentarios.
        /// </summary>
        /// <value>
        /// The comentarios.
        /// </value>
        public string Comentarios { get; set; }
        /// <summary>
        /// Gets the estatus.
        /// </summary>
        /// <value>
        /// The estatus.
        /// </value>
        public short? Estatus { get; internal set; }
        /// <summary>
        /// The s estatus
        /// </summary>
        public string sEstatus;
        /// <summary>
        /// Gets or sets the creado por.
        /// </summary>
        /// <value>
        /// The creado por.
        /// </value>
        public Nullable<int> CreadoPor { get; set; }
        /// <summary>
        /// Gets or sets the creado.
        /// </summary>
        /// <value>
        /// The creado.
        /// </value>
        public Nullable<System.DateTime> Creado { get; set; }
        /// <summary>
        /// Gets or sets the modificado por.
        /// </summary>
        /// <value>
        /// The modificado por.
        /// </value>
        public Nullable<int> ModificadoPor { get; set; }
        /// <summary>
        /// Gets or sets the modificado.
        /// </summary>
        /// <value>
        /// The modificado.
        /// </value>
        public Nullable<System.DateTime> Modificado { get; set; }
    }
}
