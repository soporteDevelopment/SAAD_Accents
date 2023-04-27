using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    /// <summary>
    /// Manages Towns of Metropolitan Area
    /// </summary>
    public class MetropolitanAreas
    {
        /// <summary>
        /// Gets the specified identifier area metropolitana.
        /// </summary>
        /// <param name="idAreaMetropolitana">The identifier area metropolitana.</param>
        /// <returns></returns>
        public tAreaMetropolitana Get(int? idAreaMetropolitana)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tAreaMetropolitanas.Find(idAreaMetropolitana);
            }
        }

        /// <summary>
        /// Gets the by town identifier.
        /// </summary>
        /// <param name="idTown">The identifier town.</param>
        /// <returns></returns>
        public tAreaMetropolitana GetByTownId(int? idTown)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tAreaMetropolitanas.Where(p=>p.idMunicipio == idTown).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the specified postal code.
        /// </summary>
        /// <param name="idTown">The identifier town.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public Tuple<int, List<tAreaMetropolitana>> Get(int? idTown, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var towns = context.tAreaMetropolitanas
                    .Where(p => p.idMunicipio.Equals(idTown) || idTown == null)
                    .Include(p => p.tMunicipio)
                    .Include(p => p.tMunicipio.tEstado)
                    .OrderByDescending(p => p.idMunicipio).Skip(page * pageSize).Take(pageSize)
                    .ToList();

                var total = context.tAreaMetropolitanas.Count();

                return Tuple.Create(total, towns);
            }
        }

        /// <summary>
        /// Adds the specified entry.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        public int Add(tAreaMetropolitana entry)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                if(GetByTownId(entry.idMunicipio) != null)
                {
                    throw new Exception("Municipio repetido");
                }

                context.tAreaMetropolitanas.Add(entry);
                context.SaveChanges();
                return entry.idAreaMetropolitana;
            }
        }

        /// <summary>
        /// Deletes the specified identifier area metropolitana.
        /// </summary>
        /// <param name="idAreaMetropolitana">The identifier area metropolitana.</param>
        /// <returns></returns>
        public int Delete(int idAreaMetropolitana)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var town = context.tAreaMetropolitanas.Find(idAreaMetropolitana);

                context.tAreaMetropolitanas.Remove(town);
                return context.SaveChanges();
            }
        }
    }
}