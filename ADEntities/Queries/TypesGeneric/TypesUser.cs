using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesUser
    {
        public static short EstatusActivo { 
            get { 
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
    }
}