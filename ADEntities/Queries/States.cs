using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace ADEntities.Queries
{
    public class States : Base
    {

        public List<StateViewModel> GetStates()
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tEstados.Select(p => new StateViewModel() { idEstado = p.id_estado, Estado = p.estado }).OrderBy(p => p.Estado).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<TownViewModel> GetTownsForState(int idState)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMunicipios.Where(p => p.estado == idState).Select(p => new TownViewModel { idMunicipio = p.id_municipio, Municipio = p.nombre_municipio }).OrderBy(p => p.Municipio).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int GetIdStateForTown(int idTown)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tMunicipios.Where(p => p.id_municipio == idTown).Select(p => p.estado).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

    }
}