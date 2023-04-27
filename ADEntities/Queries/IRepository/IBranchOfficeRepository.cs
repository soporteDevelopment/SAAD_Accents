using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// IBranchOfficeRepository
    /// </summary>
    /// <seealso cref="ADEntities.Queries.IRepository.IBaseRepository&lt;ADEntities.Models.tSucursale&gt;" />
    public interface IBranchOfficeRepository : IBaseRepository<tSucursale>
    {
        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <returns></returns>
        List<BranchOfficeViewModel> GetDetail();
    }
}
