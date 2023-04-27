using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class Commissions: Base
    {
        public CommissionViewModel GetById(int idCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tComisiones.Where(p=>p.idComision == idCommission).Select(p => new CommissionViewModel()
                    {
                        idComision = p.idComision,
                        idUsuario = p.idUsuario,
                        LimiteInferior = p.LimiteInferior,
                        LimiteSuperior = p.LimiteSuperior,
                        SueldoMensual = p.SueldoMensual,
                        PorcentajeComision = p.PorcentajeComision,
                        SueldoComision = p.SueldoComision,
                        BonoUno = p.BonoUno,
                        BonoDos = p.BonoDos,
                        BonoTres = p.BonoTres
                    }).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public List<CommissionViewModel> GetCommissions(int idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    return context.tComisiones.Where(p=>p.idUsuario == idUser).Select(p => new CommissionViewModel()
                    {

                        idComision = p.idComision,
                        idUsuario = p.idUsuario,
                        LimiteInferior = p.LimiteInferior,
                        LimiteSuperior = p.LimiteSuperior,
                        SueldoMensual = p.SueldoMensual,
                        PorcentajeComision = p.PorcentajeComision,
                        SueldoComision = p.SueldoComision,
                        BonoUno = p.BonoUno,
                        BonoDos = p.BonoDos,
                        BonoTres = p.BonoTres

                    }).OrderBy(p => p.idComision).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public int Add(tComisione commission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    context.tComisiones.Add(commission);

                    return context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }        

        public int Update(tComisione commission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entity = context.tComisiones.Where(p => p.idComision == commission.idComision).FirstOrDefault();

                    entity.LimiteInferior = commission.LimiteInferior;
                    entity.LimiteSuperior = commission.LimiteSuperior;
                    entity.SueldoMensual = commission.SueldoMensual;
                    entity.PorcentajeComision = commission.PorcentajeComision;
                    entity.SueldoComision = commission.SueldoComision;
                    entity.BonoUno = commission.BonoUno;
                    entity.BonoDos = commission.BonoDos;
                    entity.BonoTres = commission.BonoTres;

                    return context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }

        public void Remove(int idCommission)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var entity = context.tComisiones.Where(p => p.idComision == idCommission).FirstOrDefault();

                    context.tComisiones.Remove(entity);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}