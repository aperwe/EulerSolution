using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QBits.Intuition.AbstractLanguageIntelligence.Model.Brains
{
    /// <summary>
    /// Dendryt (dendritum)- element neuronu, rozgałęziona (zazwyczaj) struktura, przenosząca sygnały otrzymywane z innych neuronów przez synapsy do ciała komórki, której jest częścią. Występuje w tkance nerwowej. Słowo wywodzi się z greckiego słowa "déndron", czyli drzewo. Dendryty otrzymały taką nazwę, ponieważ przypominają gałęzie.
    /// <para/>Funkcje dendrytów: przejmują informacje z receptorów (zgodnie z kierunkiem ) od receptorów do ciała komórki.
    /// <para/>Akson (neuryt) przekazuje informacje dalej, w kierunku: od ciała komórki do narządu wykonawczego bądź zakończenia synaptycznego.
    /// <para/>
    /// http://pl.wikipedia.org/wiki/Dendryt_(biologia)
    /// </summary>
    public class Dendrite
    {
        #region Public members
        /// <summary>
        /// Cell that owns this Axon.
        /// </summary>
        public Neuron ParentCell { get; private set; }

        /// <summary>
        /// Source of signal for this dendrite.
        /// </summary>
        public Axon SignalSource { get; private set; }

        /// <summary>
        /// Weight (sensitivity) of the input signal as it travels through this dendrite into the receiving neuron cell.
        /// </summary>
        public double SignalWeight { get; private set; }

        /// <summary>
        /// Signal value received from a source axon.
        /// </summary>
        public double IncomingRawSignal { get; private set; }

        /// <summary>
        /// Weighted signal that the parent neuron 'perceives' as incoming via this dendrite.
        /// </summary>
        public double WeightedSignal { get { return IncomingRawSignal * SignalWeight; } }

        /// <summary>
        /// Event that is raised when this dendrite gets excited.
        /// </summary>
        public static event EventHandler DendriteExcited;

        #endregion

        #region Public API
        /// <summary>
        /// Creates new instance of Axon that belongs to the indicated parent cell.
        /// </summary>
        /// <param name="parentCell">Parent cell (usually the caller) that creates this axon.</param>
        /// <param name="from">Source through which signals will flow into this dendrite.</param>
        public Dendrite(Neuron parentCell, Axon from)
        {
            ParentCell = parentCell;
            SignalSource = from;
            from.SignalTarget = this;
            SignalWeight = 1.0;
        }

        /// <summary>
        /// Provides easy to read description of this dendrite.
        /// </summary>
        public override string ToString()
        {
            return string.Format("Parent={0}", ParentCell);
        }

        /// <summary>
        /// Called from the axon that is the source of the signal for this target dendrite.
        /// Modifies the weighted signal at which this dendrite is supplying signal to its parent cell.
        /// Parent neuron is excited on a separate thread to allow this to finish in a finite time without stack overflow.
        /// </summary>
        /// <param name="signal">Signal to be applied to this cell.</param>
        public void Excite(double signal)
        {
            IncomingRawSignal = signal;
            OnDendriteExcited();
            ThreadPool.QueueUserWorkItem((x) =>
                {
                    try
                    {
                        Thread.CurrentThread.Name = string.Format("Signal propagation thread from dendrite of {0}", ParentCell);
                    }
                    catch { } //Ignore errors resulting from failed thread rename.
                    ParentCell.Excite(WeightedSignal);
                });
        }
        #endregion

        /// <summary>
        /// Called when this dendrite has been excited. Raises the event.
        /// </summary>
        void OnDendriteExcited()
        {
            if (DendriteExcited != null) DendriteExcited(this, EventArgs.Empty);
        }

    }
}
