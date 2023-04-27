using ADEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DailyCuttingViewModel
    {
        public string PaymentMethod { get; set; }
        public List<TerminalBankViewModel> TerminalsBanks { get; set; }        
    }
}