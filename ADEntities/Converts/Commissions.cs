using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Converts
{
    public class Commissions
    {
        public static CommissionViewModel TableToModel(tComisione entity)
        {
            CommissionViewModel result = new CommissionViewModel();

            result.idComision = entity.idComision;
            result.idUsuario = entity.idUsuario;
            result.LimiteInferior = entity.LimiteInferior;
            result.LimiteSuperior = entity.LimiteSuperior;
            result.SueldoMensual = entity.SueldoMensual;
            result.PorcentajeComision = entity.PorcentajeComision;
            result.SueldoComision = entity.SueldoComision;
            result.BonoUno = entity.BonoUno;
            result.BonoDos = entity.BonoDos;
            result.BonoTres = entity.BonoTres;

            return result;
        }

        public static tComisione ModelToTable(CommissionViewModel model)
        {
            tComisione result = new tComisione();

            result.idComision = model.idComision;
            result.idUsuario = model.idUsuario;
            result.LimiteInferior = model.LimiteInferior;
            result.LimiteSuperior = model.LimiteSuperior;
            result.SueldoMensual = model.SueldoMensual;
            result.PorcentajeComision = model.PorcentajeComision;
            result.SueldoComision = model.SueldoComision;
            result.BonoUno = model.BonoUno;
            result.BonoDos = model.BonoDos;
            result.BonoTres = model.BonoTres;

            return result;
        }
    }
}