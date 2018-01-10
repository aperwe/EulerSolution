using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.ProbabilityClouds.Meshes
{
    class Coordinate
    {
        internal Coordinate(int rank)
        {
            dimensions = new List<double>();
            Random stat = new Random((int)DateTime.Now.Ticks);
            for (int dim = 0; dim < rank; dim++)
            {
                dimensions.Add(stat.NextDouble());
            }
        }
        List<double> dimensions;
        /// <summary>
        /// Rank (number of dimensions) of this node (should be the same as the rank of the containing space/mesh).
        /// </summary>
        internal int rank
        {
            get
            {
                return dimensions.Count;
            }
        }
    }
}
