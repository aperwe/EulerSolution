using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems
{
    /// <summary>
    /// This attribute describes the problem solver class as loaded from the problem-solver assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProblemSolverAttribute : Attribute
    {
        public string checkName { get; set; }
        public string displayName { get; set; }
        public string problemDefinition { get; set; }

        /// <summary>
        /// Constructor of an attribute
        /// </summary>
        /// <param name="checkName">Name of the check class.</param>
        /// <param name="displayName">Display name, as shown eg. on UI button.</param>
        /// <param name="problemDefinition">Definition of the problem, as described on projecteuler.net</param>
        public ProblemSolverAttribute(string checkName, string displayName = "", string problemDefinition = "")
        {
            this.checkName = checkName;
            this.displayName = displayName;
            this.problemDefinition = problemDefinition;       
        }

        public string PositionalString
        {
            get { return checkName; }
        }
    }
}
