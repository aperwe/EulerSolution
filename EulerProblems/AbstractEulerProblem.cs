using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems
{
    public abstract class AbstractEulerProblem
    {
        protected abstract void Solve(out string answer);
        private DateTime start;
        private TimeSpan elapsedTime;
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

        /// <summary>
        /// Call this method to start finding solution and get the answer.
        /// After solution is found <see cref="Answer"/> string will be set by the problem solver implementation.
        /// </summary>
        public void StartSolving()
        {
            start = DateTime.Now;
            string tempAnswer;
            Solve(out tempAnswer);
            elapsedTime = DateTime.Now - start;


            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Elapsed computation time: {0}.", elapsedTime); stringBuilder.AppendLine();
            stringBuilder.AppendLine(tempAnswer);
            Answer = stringBuilder.ToString();
        }

    }

    public class AnswerAgr : EventArgs
    {
        public string Answer { get; internal set; }
    }
}
