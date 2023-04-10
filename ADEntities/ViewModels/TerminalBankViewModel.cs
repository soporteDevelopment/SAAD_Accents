using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class TerminalBankViewModel
    {
        public string BankDestiny { get; set; }
        public string TerminalDestiny { get; set; }
        public List<CorteDiario_Result> DailyCuttings { get; set; }
    }
}