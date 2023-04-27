using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ProductOrderViewModel
    {

        public int idOrdenProducto { get; set; }

        public int idOrden { get; set; }

        public string Orden { get; set; }

        public string Factura { get; set; }

        public int idSucursal { get; set; }

        public int idProducto { get; set; }

        public string Producto { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioCompra { get; set; }

        public decimal PrecioVenta { get; set; }

        public string urlImagen { get; set; }

        public List<QuantityProductsOrderViewModel> _CantidadProductos = new List<QuantityProductsOrderViewModel>();

        public IReadOnlyCollection<QuantityProductsOrderViewModel> CantidadProductos
        {
            get { return _CantidadProductos.AsReadOnly(); }
        }

        public void AgregarCantidadProductos(QuantityProductsOrderViewModel oCantidadProductos)
        {
            _CantidadProductos.Add(oCantidadProductos);
        }

    }
}