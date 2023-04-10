using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// ViewModel of CreditNoteRequest
    /// </summary>
    public class CreditNoteRequestViewModel
    {
        /// <summary>
        /// Gets or sets the identifier tipo nota credito.
        /// </summary>
        /// <value>
        /// The identifier tipo nota credito.
        /// </value>
        public int idTipoNotaCredito { get; set; }
        /// <summary>
        /// Gets or sets the identifier venta.
        /// </summary>
        /// <value>
        /// The identifier venta.
        /// </value>
        public int idVenta { get; set; }        
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Cantidad { get; set; }
        /// <summary>
        /// Gets or sets the comentarios.
        /// </summary>
        /// <value>
        /// The comentarios.
        /// </value>
        public string Comentarios { get; set; }
        /// <summary>
        /// Gets or sets the identifier cliente fisico.
        /// </summary>
        /// <value>
        /// The identifier cliente fisico.
        /// </value>
        public int idClienteFisico { get; set; }
        /// <summary>
        /// Gets or sets the identifier cliente moral.
        /// </summary>
        /// <value>
        /// The identifier cliente moral.
        /// </value>
        public int idClienteMoral { get; set; }
        /// <summary>
        /// Gets or sets the identifier despacho.
        /// </summary>
        /// <value>
        /// The identifier despacho.
        /// </value>
        public int idDespacho { get; set; }
        /// <summary>
        /// Gets or sets the identifier forma pago.
        /// </summary>
        /// <value>
        /// The identifier forma pago.
        /// </value>
        public List<PaymentCreditNoteViewModel> FormaPago { get; set; }
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