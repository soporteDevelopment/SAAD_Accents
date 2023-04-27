using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DetailSaleGiftsTable
    {

        public int idDetalleVenta { get; set; }

        public int idVenta { get; set; }

        public int? idProducto { get; set; }

        public ProductViewModel oProducto { get; set; }

        public int? idServicio { get; set; }

        public int? idCredito { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public decimal? Descuento { get; set; }

        public int? Cantidad { get; set; }

        public string Credito { get; internal set; }

        public short? TipoImagen { get; internal set; }

        public string IdGUID { get; set; }

        public string Comentarios { get; set; }
    }
}