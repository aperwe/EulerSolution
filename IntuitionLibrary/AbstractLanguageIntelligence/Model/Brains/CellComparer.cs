using System;
using System.Collections.Generic;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// For performance, this custom comparer is used to determine whether two cells are equal.
    /// </summary>
    class CellComparer : IEqualityComparer<BaseCell>
    {
        #region IEqualityComparer<BaseCell> Members

        public bool Equals(BaseCell? x, BaseCell? y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null || y is null)
                return false;
            return x.UniqueID == y.UniqueID;
        }

        public int GetHashCode(BaseCell obj)
        {
            return obj.UniqueID.GetHashCode();
        }

        #endregion
    }
}
