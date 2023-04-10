using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class CustomerViewModel
    {
        public int idCliente {get; set;}
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Contrasena { get; set; }
        public string TelefonoCelular { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public short? Sexo { get; set; }
        public string Calle { get; set; }
        public string NumInt { get; set; }
        public string NumExt { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Cp { get; set; }
        public bool? Notificar { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool? Activo { get; set; }
    }
}