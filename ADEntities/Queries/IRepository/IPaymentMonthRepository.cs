﻿using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ICatalogPaymentMonthRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{ADEntities.Models.tPagoMes}" />
    public interface IPaymentMonthRepository : IBaseRepository<tPagoMes>
    {
    }
}
