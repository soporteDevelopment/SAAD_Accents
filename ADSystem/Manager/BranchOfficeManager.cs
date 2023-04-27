using ADEntities.Models;
using ADEntities.Queries;
using ADEntities.Queries.IRepository;
using ADEntities.ViewModels;
using ADSystem.Manager.IManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Manager
{
    public class BranchOfficeManager: BaseManager<tSucursale, BranchOfficeViewModel>, IBranchOfficeManager
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IBranchOfficeRepository _repository;
        /// <summary>
        /// Initializes a new instance of the <see cref="BranchOfficeManager" /> class.
        /// </summary>
        /// <param name="_repository">The repository.</param>
        public BranchOfficeManager(IBranchOfficeRepository repository) : base(repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Gets the detail by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public List<BranchOfficeViewModel> GetDetail()
        {
            return _repository.GetDetail();
        }
    }
}