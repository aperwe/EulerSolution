using QBits.Intuition.Mathematics.Geometry;


namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// Base cell class for all cells in brain. Cannot be implemented.
    /// </summary>
    public abstract class BaseCell
    {
        private static Random randomGenerator;

        /// <summary>
        /// Random generator used for randomness.
        /// </summary>
        private static Random RandomGenerator
        {
            get
            {
                if (randomGenerator == null)
                {
                    randomGenerator = new Random(DateTime.Now.GetHashCode());
                }
                return randomGenerator;
            }
        }

        #region Public members
        /// <summary>
        /// Unique identification of this cell across the whole neural network.
        /// Assigned automatically when the cell is constructed.
        /// </summary>
        public Guid UniqueID { get; private set; }

        /// <summary>
        /// Position of this cell within the world.
        /// </summary>
        public Point3D Position { get; internal set; }
        #endregion

        #region Public API
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseCell()
        {
            UniqueID = Guid.NewGuid();
        }

        /// <summary>
        /// Positions the cell randomly within the limits of this bounding cube.
        /// </summary>
        public void PositionCell(Cube boundingCube)
        {
            Position = new Point3D(Between(boundingCube.Origin.X, boundingCube.Stretch.X),
                Between(boundingCube.Origin.Y, boundingCube.Stretch.Y),
                Between(boundingCube.Origin.Z, boundingCube.Stretch.Z));
        }

        /// <summary>
        /// Displays cell's unique ID for easy distinguishing between two cell.s
        /// </summary>
        public override string ToString()
        {
            return UniqueID.ToString();
        }
        #endregion

        /// <summary>
        /// Gets a random number between the origin value and origin + stretch.
        /// </summary>
        private double Between(double origin, double stretch)
        {
            if (stretch == 0) return origin;

            if (stretch < 0)
            {
                origin -= stretch;
                stretch = -stretch;
            }
            var number = RandomGenerator.NextDouble() * stretch;
            return origin + number;
        }

    }
}
