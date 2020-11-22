using QBits.Intuition.Collections;
using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EulerProblems.Problems.AdditionalProblems
{
    /// <summary/>
    [ProblemSolver("Persistence problem", "Additional 001a",
@"Multiplication number persistence (MNP)
Based on Numberfile article on number persistence: https://www.youtube.com/watch?v=Wim9WJeDTHQ&ab_channel=Numberphile
for number n calculate how many steps before it reaches 0 or single digit following this 
let n = 3456
Step1: n = pers(n) => 3 * 4 * 5 * 6 => 360
Step2: n = pers(n) => 3 * 6 * 0 => 0
END
return number of steps(2).
Parallelized.
")]
    public class AdditionalProblem001a : AbstractEulerProblem
    {
        UInt64 bigInteger = 278311911269204; //<current max (program still running)
        //                  278311911269204
        object Locker = new object();
        bool ThreadContinueFlag = true; //Set to false to stop parallel tasks.
        StringBuilder stringBuilder = new StringBuilder(); //Status message.

        Queue<IEnumerable<ulong>> inputBatches = new Queue<IEnumerable<ulong>>(); //Input thread will be feeding this with new processing batches.
        UInt64 increment = 700000003;  //1 processing chunk for output thread
        List<NumberPersistenceCandidate> interestingCandidates = new List<NumberPersistenceCandidate>();
        protected override void Solve(out string answer)
        {
            answer = "";
            UpdateProgress($"Solution not created yet...");

            int persistence = 0;
            int maxPersistence = 0;

            #region Create input generator task
            var inputTask = Task.Run(InputGenerator);
            #endregion

            #region Create output processing tasks
            List<Task> outputTasks = new List<Task>();
            //We have 16 cores to execute, leave 1 for OS, 1 for input, 1 for misc.
            foreach (int i in Enumerable.Range(1, 13)) outputTasks.Add(Task.Run(OutputGenerator)); //12 processing threads because we have 16 threads (core i9).
            #endregion

            #region Create sorting task
            var sortingTask = Task.Run(AnalyzeMaxCandidatesAndRemoveLesserCandidates);
            #endregion


            #region Wait for all work to finish (now - it will finish never, unless you kill the application)
            inputTask.Wait();
            outputTasks.ForEach(t => t.Wait());
            sortingTask.Wait();
            #endregion

            UpdateProgress($"Multiplication number persistence of ({bigInteger}) = {persistence}.");
        }

        /// <summary>Thread1: Makes sure that queue is filled with at least 100 units of computation</summary>
        private void InputGenerator()
        {
            while (ThreadContinueFlag)
            {
                int newBatches = 0;
                while (inputBatches.Count < 100) //If length of input queue is shorter than target, populate it
                {
                    var tnew = Enumerable64.Range(bigInteger, increment);
                    bigInteger += increment;
                    lock (Locker)
                    {
                        inputBatches.Enqueue(tnew);
                        newBatches++;
                    }
                }
                if (newBatches > 0) AddStatusThreadsafe($"Input thread added {newBatches} new batches.");
                Task.Delay(TimeSpan.FromSeconds(30)).Wait();
            }
            AddStatusThreadsafe($"Input task finished.");
        }

        /// <summary>Thread2-n: Makes sure the queue is depleted.</summary>
        private void OutputGenerator()
        {
            while (ThreadContinueFlag)
            {
                IEnumerable<ulong> workItem; //Item take off the queue
                while (inputBatches.Count > 0)
                {
                    lock (Locker)
                    {
                        workItem = inputBatches.Dequeue();
                    }
                    int currentPersistence = 0;
                    int maxPersistence = 0;
                    UInt64 numberWithMaxPersistence = 0; //Remember this at the end of the thread.
                    AddStatusThreadsafe($"Starting batch at {workItem.First()}. Batch length {workItem.Count()}.");

                    foreach (UInt64 i in workItem)
                    {
                        currentPersistence = Persistence(i);
                        if (currentPersistence > maxPersistence)
                        {
                            AddStatusThreadsafe($"Multiplication number persistence of ({i}) = {currentPersistence}. Elapsed time: {ElapsedTime}");
                            maxPersistence = currentPersistence;
                            numberWithMaxPersistence = i;
                            var dupa = new NumberPersistenceCandidate
                            {
                                TimeStamp = DateTime.Now,
                                NumberChecked = numberWithMaxPersistence,
                                PersistenceFound = currentPersistence,
                                RangeStart = workItem.First(),
                                RangeEnd = workItem.Last()
                            };
                            lock (interestingCandidates) interestingCandidates.Add(dupa);
                        }
                    }
                    AddStatusThreadsafe($"Thread finished. Max persistence of ({maxPersistence}) for ({numberWithMaxPersistence}). Max checked: [{workItem.Last()}]. Elapsed time: {ElapsedTime}");
                    //Here: Do all the work on the given range
                    //Step 1: Find greatest (MNP)
                    //Step 2: Add Greatest MNP + Value to ListX as finding
                    //Step 3: Have another thread sort greatest MNP+Value
                }
                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            }
        }

        /// <summary>Adds status line on top of status box</summary>
        private void AddStatusThreadsafe(string status)
        {
            lock (stringBuilder)
            {
                stringBuilder.Insert(0, $"[Task {Task.CurrentId}] {status}\n");
                UpdateProgress(stringBuilder.ToString());
            }
        }
        private void AnalyzeMaxCandidatesAndRemoveLesserCandidates()
        {
            while (ThreadContinueFlag)
            {
                Task.Delay(TimeSpan.FromMinutes(5)).Wait(); //Do update every 10 mins or so
                lock (interestingCandidates)
                {
                    var countBefore = interestingCandidates.Count;

                    var maxPersistence =0;
                    var dupa = from x in interestingCandidates
                               orderby x.PersistenceFound descending
                               select x;
                    maxPersistence = dupa.First().PersistenceFound;
                    var maxPersistencePearls = from x in interestingCandidates
                              where x.PersistenceFound == maxPersistence
                              orderby x.NumberChecked ascending
                              select x;
                    AddStatusThreadsafe($"Found {countBefore} items. Pruning shorter.");
                    interestingCandidates = (from x in maxPersistencePearls select x).Take(7).ToList();
                    AddStatusThreadsafe($"Leaving in result queue [{interestingCandidates.Count}] maximum of: [{interestingCandidates.First().NumberChecked}] with persistence [{interestingCandidates.First().PersistenceFound}].");
                }
            }
        }

        //private int Persistence(BigInteger bigInteger)
        private int Persistence(UInt64 bigInteger)
        {
            if (bigInteger < 10) return 0;

            int steps = 0;
            //BigInteger current = bigInteger;
            UInt64 current = bigInteger;
            while (current > 9) //repeat while current multiplication has 2 or more digits
            {
                current = Multiply(current);
                steps++;
            }

            return steps;
        }
        /// <summary>Mutiplies digits in <paramref name="current"/> and returns the multiplication result.</summary>
        /// <param name="current"></param>
        /// <returns>Digit multiplication result</returns>
        //private BigInteger Multiply(BigInteger current)
        private UInt64 Multiply(UInt64 current)
        {
            var bytes = current.ToString().ToArray().Select((b) => b - '0');

            #region Optimization
            if (bytes.Any(b => b == 0)) return 0;
            if (bytes.Any(b => b == 5))
            {
                if (bytes.Any(b => b == 2)) return 10;
                if (bytes.Any(b => b == 4)) return 10;
                if (bytes.Any(b => b == 8)) return 10;
            }
            #endregion

            UInt64 retVal = 1;
            foreach (var b in bytes) retVal *= (UInt64)b;
            return retVal;
        }

        /// <summary>Record of maximum persistence found in some set.</summary>
        private class NumberPersistenceCandidate
        {
            public DateTime TimeStamp { get; internal set; }
            public ulong NumberChecked { get; internal set; }
            public int PersistenceFound { get; internal set; }
            public ulong RangeStart { get; internal set; }
            public ulong RangeEnd { get; internal set; }
        }
    }
}

