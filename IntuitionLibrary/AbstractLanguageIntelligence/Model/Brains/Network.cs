using QBits.Intuition.Mathematics.Geometry;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// Neural network. All neural cells must belong to a network.
    /// Two neural cells from two networks cannot communicate with each other.
    /// </summary>
    public class Network
    {
        #region Private members
        /// <summary>
        /// All cells in this network.
        /// </summary>
        private ICollection<BaseCell> Cells = new HashSet<BaseCell>(new CellComparer());

        private InputLayer inputLayer;
        private OutputLayer outputLayer;

        /// <summary>
        /// Internal collection of middle layers of the neural network.
        /// </summary>
        protected internal ICollection<MiddleLayer> MiddleLayers = new HashSet<MiddleLayer>();

        /// <summary>
        /// Random number generator used by this network.
        /// </summary>
        private readonly Random PrivateGenerator;
        #endregion

        #region Public members

        /// <summary>
        /// Gets all middle layers in this network.
        /// </summary>
        public IEnumerable<MiddleLayer> AllMiddleLayers { get { return MiddleLayers.ToArray(); } }
        /// <summary>
        /// Gets the number of cells in this network. In complex networks this can be huge (billions).
        /// </summary>
        public long CellCount
        {
            get
            {
                return Cells.Count;
            }
        }


        /// <summary>
        /// Name of this network.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the volume in space in which the network is located.
        /// </summary>
        public Cube NetworkVolume { get; set; }

        /// <summary>
        /// Input layer of this network. Created on first access.
        /// <para/>Any network may - but doesn't have to - contain an input layer.
        /// </summary>
        public InputLayer InputLayer
        {
            get
            {
                if (inputLayer == null)
                {
                    inputLayer = new InputLayer(this);
                }
                return inputLayer;
            }
        }

        /// <summary>
        /// Output layer of this network. Created on first access.
        /// <para/>Any network may - but doesn't have to - contain an output layer.
        /// </summary>
        public OutputLayer OutputLayer
        {
            get
            {
                if (outputLayer == null)
                {
                    outputLayer = new OutputLayer(this);
                }
                return outputLayer;
            }
        }
        #endregion

        #region Public API
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Network()
        {
            NetworkVolume = new Cube { Origin = new Point3D(0, 0, 0), Stretch = new Vector3D(10, 10, 10) };
            PrivateGenerator = new Random((int)DateTime.Now.Ticks);
        }

        /// <summary>
        /// Constructor that initializes the name of this network.
        /// </summary>
        /// <param name="networkName"></param>
        public Network(string networkName) : this()
        {
            Name = networkName;
        }

        /// <summary>
        /// Creates a new neural cell that is added to this network.
        /// </summary>
        /// <returns>Reference to the newly created cell.</returns>
        public TCellType CreateCell<TCellType>() where TCellType : BaseCell, new()
        {
            TCellType newCell = new TCellType();
            newCell.PositionCell(NetworkVolume);
            Cells.Add(newCell);
            return newCell;
        }

        /// <summary>
        /// Returns the cell with the specified identifier that belongs to this network.
        /// Throws exception if there is no such cell.
        /// </summary>
        /// <param name="cellId">Identifier of the cell to find.</param>
        /// <returns>Reference to the cell.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no such cell.</exception>
        public BaseCell FindCell(Guid cellId)
        {
            return Cells.Single(c => c.UniqueID.Equals(cellId));
        }

        /// <summary>
        /// Gets a random cell from this network. Useful for random testing of signal passage.
        /// </summary>
        /// <returns>Randomly selected cell.</returns>
        public BaseCell GetRandomCell()
        {
            return Cells.ElementAt(PrivateGenerator.Next(Cells.Count));
        }

        /// <summary>
        /// Shows human-readable description of this network.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Name={0}, Size={1}", Name, CellCount);
        }

        /// <summary>
        /// Creates a new neural layer that is not an interface layer, and returns a reference to it.
        /// </summary>
        /// <returns>Reference to the new layer that has no neurons yet.</returns>
        public MiddleLayer AddMiddleLayer()
        {
            var newLayer = new MiddleLayer(this);
            MiddleLayers.Add(newLayer);
            return newLayer;
        }
        #endregion
    }
}
