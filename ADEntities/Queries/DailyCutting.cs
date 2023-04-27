using ADEntities.Models;
using ADEntities.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ADEntities.Queries
{
    public class DailyCutting : Base
    {
        public List<DailyCuttingViewModel> GetDailyCutting(DateTime queryDate, int? idBranchOffice, int? idUser)
        {
            using (var context = new admDB_SAADDBEntities())
                try
                {
                    var startDate = queryDate.Date + new TimeSpan(0, 0, 0);
                    var endDate = queryDate.Date + new TimeSpan(23, 59, 59);

                    var dailyCut = context.CorteDiario(startDate, endDate, idBranchOffice, idUser).ToList();

                    var groupsDailyCut = dailyCut.ToList().GroupBy(s => s.FormaPago);

                    List<DailyCuttingViewModel> dailyCutting = new List<DailyCuttingViewModel>();

                    foreach (var group in groupsDailyCut)
                    {
                        var payments = group.ToList();

                        if (group.Key == "TRANSFERENCIA")
                        {
                            var destinyBanks = new List<TerminalBankViewModel>();

                            foreach (var destinyBank in payments.GroupBy(p => p.BancoDestino))
                            {
                                destinyBanks.Add(new TerminalBankViewModel()
                                {
                                    TerminalDestiny = destinyBank.Key,
                                    DailyCuttings = destinyBank.ToList()
                                });
                            }
                            dailyCutting.Add(new DailyCuttingViewModel()
                            {
                                PaymentMethod = group.Key,
                                TerminalsBanks = destinyBanks
                            });
                        }
                        else if (group.Key == "TARJETADEBITO" || group.Key == "TARJETACREDITO")
                        {
                            var terminalsBanks = new List<TerminalBankViewModel>();

                            foreach (var terminalBank in payments.GroupBy(p => p.TerminalDestino))
                            {
                                terminalsBanks.Add(new TerminalBankViewModel()
                                {
                                    TerminalDestiny = terminalBank.Key,
                                    DailyCuttings = terminalBank.ToList()
                                });
                            }

                            dailyCutting.Add(new DailyCuttingViewModel()
                            {
                                PaymentMethod = group.Key,
                                TerminalsBanks = terminalsBanks
                            });
                        }
                        else
                        {
                            var terminalsBanks = new List<TerminalBankViewModel>();

                            terminalsBanks.Add(new TerminalBankViewModel()
                            {
                                DailyCuttings = payments
                            });

                            dailyCutting.Add(new DailyCuttingViewModel()
                            {
                                PaymentMethod = group.Key,
                                TerminalsBanks = terminalsBanks
                            });
                        }
                    }

                    return dailyCutting;
                }
                catch (DbEntityValidationException ex)
                {
                    var newException = new ADEntities.Common.FormattedDbEntityValidationException(ex);
                    throw newException;
                }
        }
    }
}