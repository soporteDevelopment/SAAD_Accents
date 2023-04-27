using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class CatalogProviderViewModel
    {
        public int idProveedor { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string SitioWeb { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool Nacional { get; set; } = false;
        public string Domicilio { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}