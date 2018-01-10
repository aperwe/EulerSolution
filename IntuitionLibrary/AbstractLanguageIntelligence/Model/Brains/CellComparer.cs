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

        bool IEqualityComparer<BaseCell>.Equals(BaseCell x, BaseCell y)
        {
            return x.UniqueID == y.UniqueID;
        }

        int IEqualityComparer<BaseCell>.GetHashCode(BaseCell theCell)
        {
            return theCell.UniqueID.GetHashCode();
        }

        #endregion
    }
}
