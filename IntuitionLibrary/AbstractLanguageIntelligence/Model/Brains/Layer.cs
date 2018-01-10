using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// Neural network layer. Cells in a network can be grouped in layers (but don't have to).
    /// Layers are a logical organization, but are not required.
    /// </summary>
    public abstract class Layer
    {
        #region Protected members

        /// <summary>
        /// All cells in this layer.
        /// </summary>
        protected ICollection<BaseCell> LayerCells = new HashSet<BaseCell>(new CellComparer());

        /// <summary>
        /// Each layer needs to belong to a network. This is a requirement.
        /// </summary>
        public Network Owner { get; set; }
        #endregion

        #region Public members

        /// <summary>
        /// Gets the number of cells in this neuron layer.
        /// </summary>
        public int CellCount
        {
            get
            {
                return LayerCells.Count;
            }
        }

        /// <summary>
        /// Gets enumeration of all cells in this layer.
        /// </summary>
        public IEnumerable<BaseCell> Cells
        {
            get
            {
                return LayerCells.ToArray();
            }
        }
        #endregion

        #region Public API
        /// <summary>
        /// Creates an new layer by 
        /// </summary>
        /// <param name="owner">Network that this layer belongs to.</param>
        protected internal Layer(Network owner)
        {
            Debug.Assert(owner != null, "owner is null.");
            Owner = owner;
        }

        /// <summary>
        /// Adds a constructed cell to the specified network.
        /// </summary>
        /// <typeparam name="TCell">Type of the cell being added.</typeparam>
        /// <param name="cell">Cell being added.</param>
        public void AddCell<TCell>(TCell cell) where TCell : BaseCell
        {
            LayerCells.Add(cell);
        }

        /// <summary>
        /// Returns a human-readable description of this object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Owner: [{0}], Size: {1}", Owner, LayerCells.Count);
        }
        #endregion
    }
}
