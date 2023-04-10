using ADEntities.ViewModels.VirtualStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class SaleOnLineViewModel
    {
        public int IdVenta { get; set; }
        public string Remision { get; set; }
        public int? IdCarrito { get; set; }
        public int? IdCliente { get; set; }
        public int? IdTipoEntrega { get; set; }
        public string TipoEntrega { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? CantidadProductos { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Descuento { get; set; }
        public decimal? GastosEnvio { get; set; }
        public decimal? Total { get; set; }
        public string NumeroFactura { get; set; }
        public int? Estatus { get; set; }
        public string NombreEstatus { get; set; }
        public string IdOrdenOpenPay { get; set; }
        public string EstatusOpenPay { get; set; }
        public bool? Facturado { get; set; }
        public int? IdVendedor { get; set; }
        public int? IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public string EmpresaEnvio { get; set; }
        public string NumeroGuia { get; set; }
        public CustomerVirtualStoreViewModel Cliente { get; set; }
        public UserViewModel Vendedor { get; set; }
        public List<DetailSaleOnLineViewModel> Productos { get; set; }
    }
}