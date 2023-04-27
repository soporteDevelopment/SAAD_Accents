using System;

namespace ADEntities.ViewModels
{
    public class MoralCustomerViewModel
    {

        public string Nombre { get; set; }

        public string Calle { get; set; }

        public string NumExt { get; set; }

        public string NumInt { get; set; }

        public string Colonia { get; set; }

        public int? idMunicipio { get; set; }

        public int idEstado { get; set; }

        public int? CP { get; set; }

        public short? Nacionalidad { get; set; }

        public string RFC { get; set; }

        public string TelefonoCelular { get; set; }

        public string Telefono { get; set; }

        public string Correo { get; set; }

        public string SitioWeb { get; set; }

        public string NombreContacto { get; set; }

        public string TelefonoContacto { get; set; }

        public string CorreoContacto { get; set; }

        public short? Credito { get; set; }

        public int? Plazo { get; set; }

        public decimal? LimiteCredito { get; set; }

        public DateTime? FechaAlta { get; set; }

        public int? idCliente { get; set; }

        public int? idOrigen { get; set; }

    }
}