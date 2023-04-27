using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.Queries.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ADSystem.Manager.IManager.IBaseManager{tSucursale,BranchStoreViewModel}" />
    public interface IBranchManager : IBaseManager<tSucursale, BranchStoreViewModel>
    {
    }
}
