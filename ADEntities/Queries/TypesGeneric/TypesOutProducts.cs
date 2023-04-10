using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesOutProducts
    {

        public static short Todos
        {
            get
            {
                return 0;
            }
        }

        public static short Pendiente
        {
            get
            {
                return 1;
            }
        }

        public static short Entregado
        {
            get
            {
                return 2;
            }
        }
        
    }

    public class TypesOutProductsDetail
    {
        public static short Pendiente
        {
            get
            {
                return 1;
            }
        }

        public static short Entregado
        {
            get
            {
                return 2;
            }
        }

        public static short Venta
        {
            get
            {
                return 3;
            }
        }
    }

    public class TypesOutFleteEstatus
    {

        public static short Pendiente
        {
            get
            {
                return 0;
            }
        }

        public static short Aplicado
        {
            get
            {
                return 1;
            }
        }

    }
}
