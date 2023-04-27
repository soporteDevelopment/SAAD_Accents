using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class CommissionViewModel
    {
        public int idComision { get; set; }
        public int idUsuario { get; set; }
        public Nullable<decimal> LimiteInferior { get; set; }
        public Nullable<decimal> LimiteSuperior { get; set; }
        public Nullable<decimal> SueldoMensual { get; set; }
        public Nullable<decimal> PorcentajeComision { get; set; }
        public Nullable<decimal> SueldoComision { get; set; }
        public Nullable<decimal> BonoUno { get; set; }
        public Nullable<decimal> BonoDos { get; set; }
        public Nullable<decimal> BonoTres { get; set; }

    }
}