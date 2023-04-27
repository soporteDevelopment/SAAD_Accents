namespace ADEntities.ViewModels
{
    public class ProductSaleViewModel
    {
        
        public int? idProducto { get; set; }

        public string codigo { get; set; }

        public string imagen { get; set; }

        public string desc { get; set; }

        public decimal prec { get; set; }

        public decimal descuento { get; set; }

        public decimal? cantidad { get; set; }

        public int? idServicio { get; set; }

        public int? idCredito { get; set; }

        public short credito { get; set; }

        public string comentarios { get; set; }

        public short? Tipo { get; set; }

        public int? idSucursal { get; set; }

        public int? idVista { get; set;}

        public int? idCotizacion { get; set; }

        public int? idPromocion { get; set; }

        public decimal? CostoPromocion { get; set; }

        public int? idTipoPromocion { get; set; }

        public int? idProductoPadre { get; set; }
    }
}
