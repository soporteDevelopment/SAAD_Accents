using System;

namespace ADEntities.ViewModels
{
    public class CatalogViewModel
    {
        public int idCatalogo { get; set; }
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public string Volumen { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<int> idSubcategoria { get; set; }
        public Nullable<int> idProveedor { get; set; }
        public Nullable<int> idCategoria { get; set; }
        public string Imagen { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public CategoryViewModel Categoria { get; set; }
        public Nullable<int> idCatalogoMarca { get; set; }
    }
}