using System;

namespace ADEntities.ViewModels
{
    public class UserViewModel
    {
        public int idUsuario { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Calle { get; set; }

        public string NumExt { get; set; }

        public string NumInt { get; set; }

        public string Colonia { get; set; }

        public int? idMunicipio { get; set; }

        public int idEstado { get; set; }

        public int? CP { get; set; }

        public string TelefonoCelular { get; set; }

        public string Telefono { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Sexo { get; set; }

        public string Correo { get; set; }

        public int? idNivelAcademico { get; set; }

        public string titulo { get; set; }

        public int idPerfil { get; set; }

        public string Contrasena { get; set; }

        public short? Estatus { get; set; }

        public string NombreCompleto { get; set; }

        public int idSucursal { get; set; }

        public short? Ventas { get; set; }
        public bool? Facturar { get; set; }

        public short? Restringido { get; set; }

        public bool? Seleccionado { get; set; }

        public BranchStoreViewModel Sucursal { get; set; }
        public bool? AtencionTicket { get; set; }

    }
}