using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// PaymentEntryViewModel
    /// </summary>
    public class PaymentEntryViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PaymentEntryViewModel"/> is direct.
        /// </summary>
        /// <value>
        ///   <c>true</c> if direct; otherwise, <c>false</c>.
        /// </value>
        public bool Direct { get; set; }
        /// <summary>
        /// Gets or sets the identifier payment.
        /// </summary>
        /// <value>
        /// The identifier payment.
        /// </value>
        public int idPayment { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PaymentEntryViewModel"/> is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if selected; otherwise, <c>false</c>.
        /// </value>
        public bool Selected { get; set; }
    }
}