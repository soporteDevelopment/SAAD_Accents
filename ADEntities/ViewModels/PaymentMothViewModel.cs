using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// PaymentMothViewModel
    /// </summary>
    public class PaymentMothViewModel
    {
        /// <summary>
        /// Gets or sets the identifier pago meses.
        /// </summary>
        /// <value>
        /// The identifier pago meses.
        /// </value>
        public int idPagoMeses { get; set; }
        /// <summary>
        /// Gets or sets the meses.
        /// </summary>
        /// <value>
        /// The meses.
        /// </value>
        public Nullable<int> Meses { get; set; }
        /// <summary>
        /// Gets or sets the activo.
        /// </summary>
        /// <value>
        /// The activo.
        /// </value>
        public Nullable<bool> Activo { get; set; }
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