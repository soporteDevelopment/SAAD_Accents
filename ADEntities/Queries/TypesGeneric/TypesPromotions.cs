using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesPromotions
    {
        public enum TypesPromotion
        {
            Discount = 1,
            Combo = 2
        }

        public const int Discount = 1;

        public const int Combo = 2;

        public const int SpecialCombo = 3;

        public const bool Active = true;
    }
}