using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
    /// <summary>
    /// IBaseManager
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="UViewModel">The type of the view model.</typeparam>
    public interface IBaseManager<TEntity, UViewModel>
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        List<UViewModel> Get();
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        UViewModel GetById(int id);
        /// <summary>
        /// Posts the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        /// <returns></returns>
        UViewModel Post(UViewModel model);
        /// <summary>
        /// Patches the specified entity.
        /// </summary>
        /// <param name="model">The entity.</param>
        void Patch(int id, UViewModel model);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(int id);
        /// <summary>
        /// Prepares the add data.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        TEntity PrepareAddData(UViewModel viewModel);
        /// <summary>
        /// Prepares the update data.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        TEntity PrepareUpdateData(TEntity entity, UViewModel viewModel);
        /// <summary>
        /// Prepares the single return.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        UViewModel PrepareSingleReturn(TEntity entity);
        /// <summary>
        /// Prepares the multiple return.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        List<UViewModel> PrepareMultipleReturn(List<TEntity> entities);
    }
}
