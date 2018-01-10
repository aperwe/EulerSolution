using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.DesignPatterns.Observer
{
    interface IObserver
    {
        void update(IObservable o, object message);
    }
}
