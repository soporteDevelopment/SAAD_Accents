using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels.VirtualStore
{
    public class CustomerVirtualStoreViewModel: AddressViewModel
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Contrasena { get; set; }
        public string TelefonoCelular { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public short? Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public DateTime? FechaAlta { get; set; }
        public bool? Activo { get; set; }
        public bool? Notificar { get; set; }
    }
}