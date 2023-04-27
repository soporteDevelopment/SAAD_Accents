using ADEntities.Models;
using ADEntities.ViewModels;
using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    public class Banners
    {
        public static tBanner PrepareAdd(BannerViewModel entity)
        {
            tBanner result = new tBanner();

            result.Nombre = entity.Nombre;
            result.Imagen = entity.Imagen;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.Estatus = true;

            return result;
        }

        public static tBanner PrepareUpdate(BannerViewModel entity)
        {
            tBanner result = new tBanner();

            result.idBanner = entity.idBanner;
            result.Nombre = entity.Nombre;
            result.Imagen = entity.Imagen;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;
            result.Estatus = true;

            return result;
        }

        public static BannerViewModel TableToModel(tBanner entity)
        {
            BannerViewModel result = new BannerViewModel();

            result.idBanner = entity.idBanner;
            result.Nombre = entity.Nombre;
            result.Imagen = entity.Imagen;
            result.CreadoPor = entity.CreadoPor;
            result.Creado = entity.Creado;
            result.ModificadoPor = entity.ModificadoPor;
            result.Modificado = entity.Modificado;
            result.Estatus = entity.Estatus;

            return result;
        }

        public static tBanner ModelToTable(BannerViewModel model)
        {
            tBanner result = new tBanner();

            result.idBanner = model.idBanner;
            result.Nombre = model.Nombre;
            result.Imagen = model.Imagen;
            result.CreadoPor = model.CreadoPor;
            result.Creado = model.Creado;
            result.ModificadoPor = model.ModificadoPor;
            result.Modificado = model.Modificado;
            result.Estatus = model.Estatus;

            return result;
        }
    }
}