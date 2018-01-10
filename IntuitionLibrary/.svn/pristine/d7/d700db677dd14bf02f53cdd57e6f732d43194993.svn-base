using System;
using System.Collections.Generic;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// http://pl.wikipedia.org/wiki/Neuron
    /// </summary>
    public class Neuron : BaseCell
    {
        #region Private members
        /// <summary>
        /// Inputs to this neuron.
        /// </summary>
        private ICollection<Dendrite> Inputs = new HashSet<Dendrite>();

        /// <summary>
        /// Outputs of this neuron.
        /// </summary>
        private ICollection<Axon> Outputs = new HashSet<Axon>();

        #endregion

        #region Public members
        /// <summary>
        /// Indicates whether this neuron has output connections to other neurons.
        /// </summary>
        public bool HasOutputs
        {
            get
            {
                return Outputs.Count > 0;
            }
        }

        /// <summary>
        /// Indicates the number of outputs this neuron has.
        /// </summary>
        public int OutputCount
        {
            get
            {
                return Outputs.Count;
            }
        }

        /// <summary>
        /// Event that is raised when this neuron gets excited.
        /// </summary>
        public static event EventHandler NeuronExcited;
        #endregion

        #region Public API
        /// <summary>
        /// Creates a connection between an <see cref="Axon"/> (output) of this neuron
        /// with a <see cref="Dendrite"/> (input) of the other neuron.
        /// <para/>Signals from this neuron will be able to travel to the other neuron.
        /// </summary>
        /// <param name="other">Other neuron that should receive input from this neuron</param>
        public Axon ConnectTo(Neuron other)
        {
            var newAxon = new Axon(this);
            Outputs.Add(newAxon);
            other.AcceptConnection(newAxon);
            return newAxon;
        }

        /// <summary>
        /// Shows the base name and the number of inputs and outputs.
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0} [Inputs={1}, Outputs={2}]", base.ToString(), Inputs.Count, Outputs.Count);
        }

        /// <summary>
        /// Excites the neuron by applying a signal of the specified strength (Volts?).
        /// </summary>
        /// <param name="signal">Signal to be applied to this cell.</param>
        public void Excite(double signal)
        {
            if (SignalIsStrongEnoughToFireThisNeuron(signal))
            {
                OnNeuronExcited();
                foreach (var output in Outputs)
                {
                    output.Excite(signal);
                }
            }
        }

        #endregion

        /// <summary>
        /// Accepts a connection between one neuron (output) and this neuron (input).
        /// </summary>
        /// <param name="from">Source of the connection. This cell will create a dendrite that will be connected with this axon.</param>
        /// <returns>Dendrite that accepted this connection.</returns>
        private Dendrite AcceptConnection(Axon from)
        {
            var newDendrite = new Dendrite(this, from);
            Inputs.Add(newDendrite);
            return newDendrite;
        }

        /// <summary>
        /// Function that tests whether this signal is strong enough to cause to fire that signal to this neuron's output (axons).
        /// If this method returns true, the signal should be passed on to all outputs of this neuron.
        /// If this method returns false, the neuron should suppress the signal and do nothing.
        /// </summary>
        /// <param name="signal">Signal from this neuron that should be fired or not.</param>
        private bool SignalIsStrongEnoughToFireThisNeuron(double signal)
        {
            //Some conditions to test the signal.
            return signal > 100;
        }

        /// <summary>
        /// Called when this neuron has been excited. Raises the event.
        /// </summary>
        void OnNeuronExcited()
        {
            if (NeuronExcited != null) NeuronExcited(this, EventArgs.Empty);
        }
    }
}
