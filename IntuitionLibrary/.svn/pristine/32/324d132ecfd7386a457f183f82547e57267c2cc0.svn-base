using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.DesignPatterns.Observer
{
    abstract class IObservable
    {
        internal void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }
        internal void NotifyObservers(object message)
        {
            foreach (IObserver o in observers)
            {
                o.update(this, message);
            }
        }
        List<IObserver> observers = new List<IObserver>();
    }
}
