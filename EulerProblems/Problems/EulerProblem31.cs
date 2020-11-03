using QBits.Intuition.Mathematics;
using QBits.Intuition.Mathematics.Fibonacci;
using QBits.Intuition.Mathematics.Primes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace EulerProblems.Problems
{
    /// <summary/>
    [ProblemSolver("Coin sums", displayName = "Problem 31", problemDefinition =
@"In the United Kingdom the currency is made up of pound (£) and pence (p). There are eight coins in general circulation:

1p, 2p, 5p, 10p, 20p, 50p, £1 (100p), and £2 (200p).
It is possible to make £2 in the following way:

1×£1 + 1×50p + 2×20p + 1×5p + 1×2p + 3×1p
How many different ways can £2 be made using any number of coins?"
        )]
    public class EulerProblem31 : AbstractEulerProblem
    {
        protected override void Solve(out string answer)
        {
            //Max value 200p (2pound)
            int maxPocketValue = (int)CoinValues._2pound;
            List<Pocket> listOfPockets = new List<Pocket>();

            //Iterate over all combinations of pocket value to see which one produces value of exactly 2 pounds
            foreach (int twopound in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._2pound) + 1))
            {
                Pocket pocket = new Pocket();
                pocket.SetCoins(CoinValues._2pound, twopound);
                if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                if (pocket.PocketValue >= maxPocketValue) break;
                foreach (int onepound in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._pound) + 1))
                {
                    pocket.SetCoins(CoinValues._pound, onepound);
                    if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                    if (pocket.PocketValue >= maxPocketValue) break;
                    foreach (int halfpound in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._50p) + 1))
                    {
                        pocket.SetCoins(CoinValues._50p, halfpound);
                        if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                        if (pocket.PocketValue >= maxPocketValue) break;
                        foreach (int twentyp in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._20p) + 1))
                        {
                            pocket.SetCoins(CoinValues._20p, twentyp);
                            if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                            if (pocket.PocketValue >= maxPocketValue) break;
                            foreach (int tenp in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._10p) + 1))
                            {
                                pocket.SetCoins(CoinValues._10p, tenp);
                                if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                                if (pocket.PocketValue >= maxPocketValue) break;
                                foreach (int fivep in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._5p) + 1))
                                {
                                    pocket.SetCoins(CoinValues._5p, fivep);
                                    if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                                    if (pocket.PocketValue >= maxPocketValue) break;
                                    foreach (int twop in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._2p) + 1))
                                    {
                                        pocket.SetCoins(CoinValues._2p, twop);
                                        if (pocket.HasDesiredValue) listOfPockets.Add((Pocket)pocket.Clone());
                                        if (pocket.PocketValue >= maxPocketValue) break;
                                        foreach (int onep in Enumerable.Range(0, (maxPocketValue / (int)CoinValues._1p) + 1))
                                        {
                                            pocket.SetCoins(CoinValues._1p, onep);
                                            if (pocket.HasDesiredValue)
                                            {
                                                listOfPockets.Add((Pocket)pocket.Clone());
                                                break;
                                            }
                                            if (pocket.PocketValue >= maxPocketValue) break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            answer = string.Format("Still computing... Number of pockets {0}.", listOfPockets.Count);
        }
    }
    enum CoinValues
    {
        _1p = 1, //1 penny
        _2p = 2, //2 penny
        _5p = 5, //5 penny
        _10p = 10, //10 penny
        _20p = 20, //20 penny
        _50p = 50, //50 penny
        _pound = 100, //1 pound
        _2pound = 200 //2 pound
    }

    /// <summary>
    /// Object representing a pocket containing set of coins of the specific values.
    /// </summary>
    public class Pocket : ICloneable
    {
        Dictionary<CoinValues, int> purse; //Pocket contents

        public Pocket()
        {
            purse = new Dictionary<CoinValues, int>();
        }
        public int PocketValue
        {
            get
            {
                int value = 0;
                foreach (var pair in purse)
                {
                    value += (int)pair.Key * pair.Value;
                }
                return value;
            }
        }
        public bool HasDesiredValue
        {
            get
            {
                return PocketValue == 200;
            }
        }

        /// <summary>
        /// Adds coins of the specified denomination to the pocket.
        /// </summary>
        /// <param name="coinType">Coin denomination.</param>
        /// <param name="numCoins">Number of coins to add.</param>
        /// <param name="fast">Set to true when cloning for fast copy operation.</param>
        internal void SetCoins(CoinValues coinType, int numCoins, bool fast = false)
        {
            purse[coinType] = numCoins;
            if (!fast)
            {
                //Cleanup the purse of all smaller denomination for correct behavior.
                List<CoinValues> valuesToRemove = new List<CoinValues>();
                foreach (var pair in purse)
                {
                    if (pair.Key < coinType) valuesToRemove.Add(pair.Key);
                }
                valuesToRemove.ForEach(value => purse.Remove(value));
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(PocketValue.ToString());
            foreach (var pair in purse)
            {
                sb.AppendFormat(" [{0}]x{1};", pair.Key, pair.Value);
            }
            return sb.ToString();
        }

        public object Clone()
        {
            Pocket copy = new Pocket();
            foreach (var pair in purse) copy.SetCoins(pair.Key, pair.Value, fast: true);
            return copy;
        }

    }

}
