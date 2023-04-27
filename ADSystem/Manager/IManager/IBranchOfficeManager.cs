using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
    public interface IBranchOfficeManager: IBaseManager<tSucursale, BranchOfficeViewModel>
    {
        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <returns></returns>
        List<BranchOfficeViewModel> GetDetail();
    }
}
