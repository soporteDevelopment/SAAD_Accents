using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class SaleGiftsTableViewModel
    {

        public int idVenta { get; set; }

        public int? idMesaRegalo { get; set; }

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

        public string Comentarios { get; set; }

        public decimal? TotalNotasCredito { get; set; }

        public string NumeroFactura { get; set; }

        public byte? TipoCliente { get; set; }

        public List<string> DescFormasPago { get; set; }

        public List<SaleDetailViewModel> oDetail { get; set; }

        public tClientesFisico oClienteFisico { get; set; }

        public tClientesMorale oClienteMoral { get; set; }

        public AddressViewModel oAddress { get; set; }

        public tDespacho oDespacho { get; set; }

        public decimal SumSubtotal { get; internal set; }

    }
}