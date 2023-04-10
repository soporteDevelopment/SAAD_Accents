using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels.VirtualStore
{
    public class BannerViewModel
    {
        public int idBanner { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public Nullable<bool> Estatus { get; set; }
    }
}