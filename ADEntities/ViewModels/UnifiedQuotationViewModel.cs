using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class UnifiedQuotationViewModel
    {
        public int? idUsuario;
        public List<QuotationViewModel> oDetail;

        public int idCotizacion { get; set; }

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

        public short? IVA { get; set; }

        public byte? TipoCliente { get; set; }

        public string Comentarios { get; set; }

        public AddressViewModel oAddress { get; set; }

        public List<QuotationViewModel> lCotizaciones { get; set; }
        public int? Customer { get; set; }
        public string Vendedor { get; set; }
        public string Vendedor2 { get; set; }        
        public decimal? IVATasa { get; set; }               
        public bool? Dolar { get; set; }
    }
}