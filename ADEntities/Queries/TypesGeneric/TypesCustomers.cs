using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesCustomers
    {

        public static byte PhysicalCustomer
        {
            get
            {
                return 0;
            }
        }

        public static byte MoralCustomer
        {
            get
            {
                return 1;
            }
        }

        public static byte OfficeCustomer
        {
            get
            {
                return 2;
            }
        }

        public static string sPhysicalCustomer
        {
            get
            {
                return "Físico";
            }
        }

        public static string sMoralCustomer
        {
            get
            {
                return "Moral";
            }
        }

        public static string sOfficeCustomer
        {
            get
            {
                return "Despacho";
            }
        }

    }
}
