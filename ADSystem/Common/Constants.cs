using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Common
{
    public class Constants
    {

        public static bool bActivo { get { return true; } }

        public static bool bInactivo { get { return false; } }

        public static short sActivo { get { return 1; } }

        public static short sInactivo { get { return 0; } }

        public static short profileAdmin { get { return 8; } }

        public static short branchAll { get { return 5; } }

        public static short Amazonas { get { return 2; } }

        public static short Guadalquivir { get { return 3; } }

        public static short Textura { get { return 4; } }

        public static short Manager { get { return 8; } }

        public static short ClienteMoral { get { return 0; } }

        public static short ClienteFisico { get { return 1; } }

        public static short ClienteOtro { get { return 2; } }

        public static short ventaSaldada { get { return 1; } }

        public static short ventaPendiente { get { return 2; } }

        public static short ventaCancelada { get { return 3; } }

        public static int outProductPendiente { get { return 1; } }

        public static int outProductEntregada { get { return 2; } }

        public Dictionary<string, int> TypesPayment { get; set; }

        public Dictionary<string, int> TypesCard { get; set; }

        public static short CompletedReception { get { return 1; } }

        public static short PendingReception { get { return 2; } }

        public static short CanceledReception { get { return 3; } }

        public Constants()
        {

            TypesPayment = new Dictionary<string, int>();

            TypesPayment.Add("Cheque", 1);
            TypesPayment.Add("Crédito", 2);
            TypesPayment.Add("Deposito", 3);
            TypesPayment.Add("Efectivo", 4);
            TypesPayment.Add("TarjetaCredito", 5);
            TypesPayment.Add("TarjetaDebito", 6);
            TypesPayment.Add("Transferencia", 7);
            TypesPayment.Add("NotaCredito", 8);
            TypesPayment.Add("Intercambio", 9);

            TypesCard = new Dictionary<string, int>();

            TypesCard.Add("AmericanExpress", 1);
            TypesCard.Add("MasterCard", 2);
            TypesCard.Add("Visa", 3);

        }
        public static int PendingStatusTicket = 1;
    }
}