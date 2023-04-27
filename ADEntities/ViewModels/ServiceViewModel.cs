using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ServiceViewModel
    {

        public int idServicio { get; set; }

        public string Descripcion { get; set; }

        public short Instalacion { get; set; }

        public bool? AplicarDescuento { get; set; }

        public int MedidasEstandar { get; set; }

        public List<ServiceMeasureStandarViewModel> oMedidasEstandar { get; set; }
    }
}