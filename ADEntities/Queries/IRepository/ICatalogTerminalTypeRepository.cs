using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// ICatalogBankAccountRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository{tCatalogoTipoTerminales}" />
    public interface ICatalogTerminalTypeRepository : IBaseRepository<tCatalogoTipoTerminale>
    {
    }
}
