using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels.VirtualStore
{
    public class AddressViewModel
    {

        public string Calle { get; set; }
        public string NumInt { get; set; }
        public string NumExt { get; set; }
        public string Colonia { get; set; }
        public int? IdMunicipio { get; set; }
        public string Municipio { get; set; }
        public int? IdEstado { get; set; }
        public string Estado { get; set; }
        public string Cp { get; set; }
        public bool? TipoDireccion { get; set; }
        public string Direccion { get; set; }
    }
}