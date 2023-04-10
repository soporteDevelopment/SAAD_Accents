namespace ADEntities.ViewModels
{
    public class ServiceOutProductViewModel
    {

        public int idServicioVista { get; set; }

        public int? idVista { get; set; }

        public int? idServicio { get; set; }

        public string Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public decimal? Cantidad { get; set; }

        public short Estatus { get; set; }

        public string Comentarios { get; set; }

    }
}
