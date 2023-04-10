using ADEntities.Models;
using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class UnifySaleViewModel
    {

        public int idVenta { get; set; }
        public byte? TipoCliente { get; set; }
        public int? Customer { get; set; }
        public int? idDespachoReferencia { get; set; }
        //public string Despacho { get; set; }
        public string Proyecto { get; set; }
        public string Vendedor { get; set; }
        public string Vendedor2 { get; set; }
        public int? idUsuario { get; set; }
        public string Comentarios { get; set; }
        public decimal? Descuento { get; set; }
        public short? IVA { get; set; }
        public List<SaleViewModel> oDetail { get; set; }
    }

    public class SaleViewModel
    {
        public int idVenta { get; set; }

        public int? idVista { get; set; }

        public string Remision { get; set; }

        public int? idUsuario1 { get; set; }

        public string Usuario1 { get; set; }

        public int? idUsuario2 { get; set; }

        public string Usuario2 { get; set; }

        public int? idClienteFisico { get; set; }

        public string ClienteFisico { get; set; }

        public int? idClienteMoral { get; set; }

        public string ClienteMoral { get; set; }

        public int? idDespacho { get; set; }

        public string Despacho { get; set; }

        public string Proyecto { get; set; }

        public int? idDespachoReferencia { get; set; }

        public string DespachoReferencia { get; set; }

        public int? idSucursal { get; set; }

        public string Sucursal { get; set; }

        public DateTime? Fecha { get; set; }

        public decimal? CantidadProductos { get; set; }

        public decimal? Subtotal { get; set; }

        public decimal? Descuento { get; set; }

        public decimal? Total { get; set; }

        public short? Estatus { get; set; }

        public string sEstatus { get; set; }

        public int? FormaPago { get; set; }

        public short? IVA { get; set; }

        public short? Factura { get; set; }

        public decimal? TotalNotasCredito { get; set; }

        public string NumberFactura { get; set; }

        public byte? TipoCliente { get; set; }

        public List<string> DescFormasPago { get; set; }

        public List<SaleDetailViewModel> oDetail { get; set; }

        public List<OutProductsDetailViewModel> oOutProductDetail { get; set; }

        public tClientesFisico oClienteFisico { get; set; }

        public tClientesMorale oClienteMoral { get; set; }

        public AddressViewModel oAddress { get; set; }

        public tDespacho oDespacho { get; set; }

        public decimal? SumSubtotal { get; internal set; }

        public string Comentarios { get; set; }

        public string RemisionVenta { get; set; }

        public bool Editar { get; set; }

        public bool? Dolar { get; internal set; }

        public bool Selected { get; set; }

        public decimal? IVATotal { get; set; }

        public decimal IVATasa { get; set; }
    }
}
