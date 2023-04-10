using ADEntities.Models;
using ADEntities.Queries.TypesGeneric;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Banners
    {        
        //Get
        public Tuple<int, List<BannerViewModel>> Get(string name, int page, int pageSize)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var banners = context.tBanners
                    .Where(p => p.Nombre.Contains(name) || String.IsNullOrEmpty(name) && p.Estatus == TypesBanners.Active)
                    .Select(p => new BannerViewModel()
                    {
                        idBanner = p.idBanner,
                        Nombre = p.Nombre,
                        Imagen = p.Imagen,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).OrderByDescending(p => p.CreadoPor).Skip(page * pageSize).Take(pageSize).ToList();

                var total = context.tBanners.Count();

                return Tuple.Create(total, banners);
            }
        }

        //GetActives
        public List<BannerViewModel> GetActives(string remission)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tBanners.Where(p => p.Estatus == TypesBanners.Active)
                    .Select(p => new BannerViewModel()
                    {
                        idBanner = p.idBanner,
                        Nombre = p.Nombre,
                        Imagen = p.Imagen,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).OrderByDescending(p => p.Creado).ToList();
            }
        }

        public BannerViewModel GetById(int id)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                return context.tBanners.Where(p => p.idBanner == id)
                    .Select(p => new BannerViewModel()
                    {
                        idBanner = p.idBanner,
                        Nombre = p.Nombre,
                        Imagen = p.Imagen,
                        Estatus = p.Estatus,
                        CreadoPor = p.CreadoPor,
                        Creado = p.Creado,
                        ModificadoPor = p.ModificadoPor,
                        Modificado = p.Modificado
                    }).FirstOrDefault();
            }
        }

        //Post
        public int Add(tBanner entry)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                context.tBanners.Add(entry);
                context.SaveChanges();
                return entry.idBanner;
            }
        }

        //Patch
        public int Update(tBanner entry)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tBanners.Find(entry.idBanner);

                entity.Nombre = entry.Nombre;
                entity.ModificadoPor = entry.ModificadoPor;
                entity.Modificado = entry.Modificado;

                return context.SaveChanges();
            }
        }

        //Delete
        public int Delete(int id, int modifiedBy, DateTime modified)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tBanners.Find(id);

                entity.ModificadoPor = modifiedBy;
                entity.Modificado = modified;
                entity.Estatus = TypesBanners.Inactive;

                return context.SaveChanges();
            }
        }

        //UpdateReport
        public int UpdateImage(int id, string image)
        {
            using (var context = new admDB_SAADDBEntities())
            {
                var entity = context.tBanners.Find(id);

                entity.Imagen = image;

                return context.SaveChanges();
            }
        }
    }
}