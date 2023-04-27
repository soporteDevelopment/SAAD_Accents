namespace ADEntities.ViewModels
{
    public class OfficeViewModel
    {

        public int idDespacho { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public string Calle { get; set; }

        public string NumExt { get; set; }

        public string NumInt { get; set; }

        public string Colonia { get; set; }

        public int? idMunicipio { get; set; }

        public int? CP { get; set; }

        public string Correo { get; set; }

        public decimal? Comision { get; set; }

        public int? idOrigen { get; set; }
    }
}
