using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.IRepository
{
    /// <summary>
    /// IBaseRepository
    /// </summary>
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        List<TEntity> Get();
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity GetById(int id);
        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        TEntity Add(TEntity model);
        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        TEntity Update(TEntity model);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        void Delete(int id);
    }
}
