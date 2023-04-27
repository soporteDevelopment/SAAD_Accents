using ADEntities.Models;
using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class GiftsTableViewModel
    {        

        public int idMesaRegalo { get; set; }

        public string Remision { get; set; }

        public int? idNovio { get; set; }

        public string Novio { get; set; }

        public string CorreoNovio { get; set; }        

        public int? idNovia { get; set; }

        public string Novia { get; set; }

        public string CorreoNovia { get; set; }

        public DateTime? FechaBoda { get; set; }

        public string LugarBoda { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public string HoraBoda { get; set; }

        public int? idVendedor1 { get; set; }

        public string Vendedor1 { get; set; }

        public int? idVendedor2 { get; set; }

        public string Vendedor2 { get; set; }

        public int? idSucursal { get; set; }

        public string Sucursal { get; set; }

        public int? idDespachoReferencia { get; set; }

        public string DespachoReferencia { get; set; }

        public int? CantidadProductos { get; set; }

        public decimal? SumSubtotal { get; internal set; }

        public decimal? Subtotal { get; set; }

        public short? IVA { get; set; }

        public decimal? Descuento { get; set; }

        public decimal? Total { get; set; }

        public short? Estatus { get; set; }

        public string sEstatus { get; set; }

        public int? idVenta { get; set; }

        public int? idNotaCredito { get; set; }

        public string Comentarios { get; set; }

        public int? Notificar { get; set; }

        public DateTime? Fecha { get; set; }

        public List<DetailGiftsTableViewModel> lDetail { get; set; }

        public List<SaleGiftsTableViewModel> lSales { get; set; }
        
    }
}