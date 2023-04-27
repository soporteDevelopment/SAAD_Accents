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
	/// ISubcategoryManager
	/// </summary>
	/// <seealso cref="ADSystem.Manager.IManager.IBaseManager&lt;ADEntities.Models.tSubcategoria, ADEntities.ViewModels.SubcategoryViewModel&gt;" />
	public interface ISubcategoryManager : IBaseManager<tSubcategoria, SubcategoryViewModel>
	{
	}
}
