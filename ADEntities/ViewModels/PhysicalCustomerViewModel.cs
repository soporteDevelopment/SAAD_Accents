using System;

namespace ADEntities.ViewModels
{
    public class PhysicalCustomerViewModel
    {

        public int idCliente { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string NombreCompleto { get; set; }

        public string Calle { get; set; }

        public string NumExt { get; set; }

        public string NumInt { get; set; }

        public string Colonia { get; set; }

        public int? idMunicipio { get; set; }

        public int? idEstado { get; set; }

        public int? CP { get; set; }

        public string TelefonoCelular { get; set; }

        public string Telefono { get; set; }

        public DateTime? FechaAlta { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Sexo { get; set; }

        public string Correo { get; set; }

        public string NoIFE { get; set; }

        public string RFC { get; set; }

        public short? Credito { get; set; }

        public int? Plazo { get; set; }

        public decimal? LimiteCredito { get; set; }

        public string NombreIntermediario { get; set; }

        public string TelefonoIntermediario { get; set; }

        public int? idOrigen { get; set; }

    }
}