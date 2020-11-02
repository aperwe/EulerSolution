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
            foreach (Type type in allTypes)
            {
                foreach (var attrib in type.GetCustomAttributes(false))
                {
                    if (attrib is ProblemSolverAttribute)
                    {
                        var psca = attrib as ProblemSolverAttribute;
                        SolverInfo solver = new SolverInfo
                        {
                            DisplayName = psca.displayName,
                            TypeInfo = type,
                            ProblemDescription = psca.problemDefinition,
                        };
                        infoList.Add(solver);
                    }
                }
            }
            return infoList.OrderBy(si => si.DisplayName);
        }
    }
}