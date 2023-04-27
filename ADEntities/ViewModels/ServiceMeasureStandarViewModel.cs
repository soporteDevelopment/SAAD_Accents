using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class ServiceMeasureStandarViewModel
    {
        public int idServicioMedidasEstandar { get; set; }
        public Nullable<int> idServicio { get; set; }
        public Nullable<int> idTipoMedida { get; set; }
        public Nullable<decimal> Valor { get; set; }

        public string NombreMedida { get; set; }

        public virtual tServicio tServicio { get; set; }
        public virtual tTipoMedida tTipoMedida { get; set; }
    }
}