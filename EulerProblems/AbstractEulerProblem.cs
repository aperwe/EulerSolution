using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems
{
    public abstract class AbstractEulerProblem
    {
        public abstract void Solve();
        public string Answer
        {
            set
            {
                if (AnswerAvailableEventHandler != null)
                {
                    var AnswerAgr = new AnswerAgr() { Answer = value };
                    AnswerAvailableEventHandler(this, AnswerAgr);
                }
            }
        }
        public event EventHandler<AnswerAgr> AnswerAvailableEventHandler;

    }
    public class AnswerAgr : EventArgs
    {
        public string Answer { get; internal set; }
    }
}
