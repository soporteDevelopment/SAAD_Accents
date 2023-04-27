using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// TownViewModel
    /// </summary>
    public class TownViewModel
    {
        public int idMunicipio { get; set; }
        public string Municipio { get; set; }
        public int idEstado { get; set; }
        public StateViewModel Estado { get; set; }
    }
}