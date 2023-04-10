using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ProductViewModel
    {
        public int idProducto { get; set; }

        public string Nombre { get; set; }

        public string NombreComercial { get; set; }

        public string Descripcion { get; set; }

        public decimal PrecioVenta { get; set; }

        public decimal? PrecioCompra { get; set; }

        public decimal? Descuento { get; set; }

        public int? idProveedor { get; set; }

        public int idCategoria { get; set; }

        public int? idSubcategoria { get; set; }

        public string Color { get; set; }

        public int? idMaterial { get; set; }

        public string Medida { get; set; }

        public decimal? Peso { get; set; }

        public string Codigo { get; set; }

        public short Estatus { get; set; }

        public string Marca { get; set; }

        public string Extension { get; set; }

        public string urlImagen { get; set; }

        public short? TipoImagen { get; set; }

        public string Proveedor { get; set; }

        public string Comentarios { get; set; }

        public decimal? Stock { get; set; }

        public decimal? Vista { get; set; }

        public decimal? Taller { get; internal set; }

        public string NombreImagen { get; set; }

        public int Cantidad { get; set; }

        public int? CantidadAnterior { get; set; }

        public int? CantidadActual { get; set; }

        public DateTime? Fecha { get; set; }

        public decimal? Total { get; set; }

        public int CreadoPor { get; set; }

        public DateTime? Creado { get; set; }

        public int ModificadoPor { get; set; }

        public DateTime? Modificado { get; set; }

        public string Imagen { get; set; }

        public ProductPromotionViewModel Promotion { get; set; }

        public List<DetailView> oDetailView { get; set; }

        public List<ProductBranchViewModel> _Existencias = new List<ProductBranchViewModel>();

        public int idSucursal { get; set; }

        public IReadOnlyCollection<ProductBranchViewModel> Existencias
        {
            get { return _Existencias.AsReadOnly(); }
        }

        public void AgregarExistencia(ProductBranchViewModel oProductBranch)
        {
            _Existencias.Add(oProductBranch);
        }

    }

    public class DetailView
    {

        public string Remision { get; set; }

        public decimal? Cantidad { get; set; }

    }

}