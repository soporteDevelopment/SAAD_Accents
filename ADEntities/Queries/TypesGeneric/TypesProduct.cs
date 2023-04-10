using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesProduct
    {
        public static short EstatusActivo
        {
            get
            {
                return 1;
            }
        }

        public static short EstatusInactivo
        {
            get
            {
                return 0;
            }
        }

        public static short SucursalTodas
        {
            get
            {
                return 1;
            }
        }

        public static short SucursalAmazonas
        {
            get
            {
                return 2;
            }
        }

        public static short SucursalGuadalquivir
        {
            get
            {
                return 1;
            }

        }

    }
}