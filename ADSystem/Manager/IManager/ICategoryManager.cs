using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADSystem.Manager.IManager
{
	/// <summary>
	/// ICategoryManager
	/// </summary>
	/// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tCategoria, ADEntities.ViewModels.CategoryViewModel&gt;" />
	public interface ICategoryManager : IBaseManager<tCategoria, CategoryViewModel>
	{
	}
}
