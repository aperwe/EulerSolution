using System;
using System.Text;
using System.Threading.Tasks;

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
        protected void UpdateProgress(string progressMessage)
        {
            Answer = progressMessage;
        }
        /// <summary>Returns currently elapsed time of solver execution</summary>
        protected TimeSpan ElapsedTime => DateTime.Now - start;
        public event EventHandler<AnswerAgr>? AnswerAvailableEventHandler;

        /// <summary>
        /// Call this method to start finding solution and get the answer.
        /// After solution is found <see cref="Answer"/> string will be set by the problem solver implementation.
        /// </summary>
        public async void StartSolving()
        {
            start = DateTime.Now;
            Solve(out string tempAnswer);
            //Task<string> longRunningTask = new Task<string>(SolveAsync);
            //string tempAnswer = await longRunningTask;
            elapsedTime = DateTime.Now - start;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Elapsed computation time: {0}.", elapsedTime); stringBuilder.AppendLine();
            stringBuilder.AppendLine(tempAnswer);
            Answer = stringBuilder.ToString();
        }
        string SolveAsync()
        {
            Solve(out string tempAnswer);
            return tempAnswer;
        }

    }

    public class AnswerAgr : EventArgs
    {
        public required string Answer { get; set; }
    }
}
