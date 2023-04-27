using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
	public class TypesPayment
	{
		public Dictionary<string, int> ListTypesPayment { get; set; }
		public TypesPayment()
		{
			ListTypesPayment = new Dictionary<string, int>();

			ListTypesPayment.Add("Cheque", 1);
			ListTypesPayment.Add("Crédito", 2);
			ListTypesPayment.Add("Deposito", 3);
			ListTypesPayment.Add("Efectivo", 4);
			ListTypesPayment.Add("TarjetaCredito", 5);
			ListTypesPayment.Add("TarjetaDebito", 6);
			ListTypesPayment.Add("Transferencia", 7);
			ListTypesPayment.Add("NotaCredito", 8);
			ListTypesPayment.Add("Intercambio", 9);
			ListTypesPayment.Add("MercadoPago", 10);
		}

		public static string Cheque { get { return "Cheque"; } }
		public static string Credito { get { return "Crédito"; } }
		public static string Deposito { get { return "Deposito"; } }
		public static string Efectivo { get { return "Efectivo"; } }
		public static string TarjetaCredito { get { return "TarjetaCredito"; } }
		public static string TarjetaDebito { get { return "TarjetaDebito"; } }
		public static string Transferencia { get { return "Transferencia"; } }
		public static string NotaCredito { get { return "NotaCredito"; } }
		public static string Intercambio { get { return "Intercambio"; } }
		public static string MercadoPago { get { return "Mercado Pago"; } }

		public static int iCheque { get { return 1; } }
		public static int iCredito { get { return 2; } }
		public static int iDeposito { get { return 3; } }
		public static int iEfectivo { get { return 4; } }
		public static int iTarjetaCredito { get { return 5; } }
		public static int iTarjetaDebito { get { return 6; } }
		public static int iTransferencia { get { return 7; } }
		public static int iNotaCredito { get { return 8; } }
		public static int iIntercambio { get { return 9; } }
		public static int iMercadoPago { get { return 10; } }

		//Status
		public static short iSaldada { get { return 1; } }
		public static short iPendiente { get { return 2; } }
		public static short iCancelada { get { return 3; } }
		public static short iCanceladaCredito { get { return 4; } }
	}
}
