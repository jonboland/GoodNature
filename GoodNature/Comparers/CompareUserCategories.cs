using GoodNature.Areas.Admin.Models;
using GoodNature.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GoodNature.Comparers
{
    public class CompareUserCategories : IEqualityComparer<UserCategory>
    {
        public bool Equals(UserCategory x, UserCategory y)
        {
            if (y == null)
            {
                return false;
            }
                
            if (x.UserId == y.UserId)
            {
                return true;
            }

            return false;
                
        }

        public int GetHashCode([DisallowNull] UserCategory obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
