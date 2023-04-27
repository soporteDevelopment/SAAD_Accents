using ADEntities.Models;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ADSystem.Manager
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ADSystem.Manager.BaseManager{ADEntities.Models.tSucursale, ADEntities.ViewModels.BranchStoreViewModel};" />
    /// <seealso cref="ADSystem.Manager.IManager.IBranchManager" />
    public class BranchManager : BaseManager<tSucursale, BranchStoreViewModel>, IBranchManager
    {
        private IBranchRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchManager"/> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public BranchManager(IBranchRepository _repository) : base(_repository)
        {
            repository = _repository;
        }

        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override BranchStoreViewModel PrepareSingleReturn(tSucursale entity)
        {
            return new BranchStoreViewModel()
            {
                idSucursal = entity.idSucursal,
                Nombre = entity.Nombre,
                DatosFiscales = entity.DatosFiscales,
                Descripcion = entity.Descripcion,
                Horarios = entity.Horarios,
                IVATasa = entity.IVATasa,
                Telefono = entity.Telefono,
                
            };
        }
    }
}