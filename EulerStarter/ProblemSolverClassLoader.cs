using EulerProblems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EulerStarter
{
    internal class ProblemSolverClassLoader
    {
        public ProblemSolverClassLoader()
        {
        }

        /// <summary>
        /// Finds and loads all problem solvers with attributes
        /// </summary>
        /// <returns>List of problem constructors</returns>
        internal IEnumerable<SolverInfo> LoadProblemSolvers()
        {
            Assembly problemAssembly = Assembly.GetAssembly(typeof(AbstractEulerProblem));
            var allTypes = problemAssembly.GetTypes();
            var infoList = new List<SolverInfo>();
            foreach (Type t in allTypes)
            {
                foreach (var a in t.GetCustomAttributes(false))
                {
                    if (a is ProblemSolverClassAttribute)
                    {
                        var psca = a as ProblemSolverClassAttribute;
                        SolverInfo si = new SolverInfo
                        {
                            DisplayName = psca.DisplayName,
                            TypeInfo = t
                        };
                        infoList.Add(si);
                    }
                }
            }
            return infoList.OrderBy(si => si.DisplayName);
        }
    }
}