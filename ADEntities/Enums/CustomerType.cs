using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Enums
{
    /// <summary>
    /// CustomerType
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// The physical customer
        /// </summary>
        PhysicalCustomer = 1,
        /// <summary>
        /// The moral customer
        /// </summary>
        MoralCustomer = 2,
        /// <summary>
        /// The office customer
        /// </summary>
        OfficeCustomer = 3
    }
}