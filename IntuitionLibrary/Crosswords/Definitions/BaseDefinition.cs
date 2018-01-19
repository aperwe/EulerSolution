using QBits.Intuition.DesignPatterns.Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QBits.Intuition.Crosswords.Definitions
{
    /// <summary>
    /// Base abstract class for any definition of a term on any crossword.
    /// </summary>
    public abstract class BaseDefinition : IComponent, ISite
    {
        static bool fConstructorsInitialized = false;
        /// <summary>
        /// Default constructor (protected).
        /// </summary>
        protected BaseDefinition()
        {
            DefinitionChangedEvt += new EventHandler(delegate(object sender, EventArgs e) { }); //Default handler, obviously doing nothing. Just to make sure raising the Event Handler does not throw exception.
        }
        /// <summary>
        /// Direction of definition.
        /// </summary>
        protected Kierunek _kier = Kierunek.Nieokreślony;
        /// <summary>
        /// Gets the direction of the definition.
        /// </summary>
        public virtual Kierunek Kierunek
        {
            get
            {
                return _kier;
            }
        }
        /// <summary>
        /// Holds the definition of the crossword element.
        /// </summary>
        protected string _definicja;
        /// <summary>
        /// Gets or sets the definition for crossword element.
        /// </summary>
        public virtual string definicja
        {
            get
            {
                return _definicja;
            }
            set
            {
                _definicja = value;
                DefinitionChangedEvt(this, null);
            }
        }
        /// <summary>
        /// Gets or sets the list of propsal solutions for the given definition.
        /// </summary>
        public List<string> propozycje = new List<string>();
        /// <summary>
        /// Event raised when definition is changed.
        /// </summary>
        public event EventHandler DefinitionChangedEvt;
        /// <summary>
        /// Creates the definition.
        /// </summary>
        /// <param name="kier">Direction of the definition.</param>
        /// <returns>Created instance.</returns>
        public static BaseDefinition CreateDefinition(Kierunek kier)
        {
            TryInitFactory();
            BaseDefinition newObject = UniversalFactory<Kierunek, BaseDefinition>.SAP.CreateObject(kier);
            return newObject;
        }
        private static void TryInitFactory()
        {
            if (fConstructorsInitialized) return;
            UniversalFactory<Kierunek, BaseDefinition>.SAP.RegisterConstructor(Kierunek.Poziomo, HorizontalDefinition.ctor);
        }

        #region IComponent Members
        /// <summary>
        /// Can be implemented in the implementor.
        /// </summary>
        public event EventHandler Disposed;
        public ISite Site
        {
            get
            {
                return this;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ISite Members

        IComponent ISite.Component
        {
            get { return this; }
        }

        IContainer ISite.Container
        {
            get { return null; }
        }

        bool ISite.DesignMode
        {
            get { return false; }
        }

        string ISite.Name
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region IServiceProvider Members

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
    delegate BaseDefinition defConstructor();
    public enum Kierunek
    {
        Nieokreślony,
        Poziomo,
        Pionowo
    }
}
