using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace QBits.Intuition.Mathematics.Geometry
{
    /// <summary>
    /// A cube in the 3D world that defines a bounding cube for positions.
    /// Used to create 
    /// </summary>
    public class Cube
    {
        /// <summary>
        /// Initial position of the cube.
        /// </summary>
        internal Point3D Origin { get; set; }

        /// <summary>
        /// Width, height and depth of the cube.
        /// </summary>
        internal Vector3D Stretch { get; set; }
    }
}
