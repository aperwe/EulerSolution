using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EulerProblems
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProblemSolverClassAttribute : Attribute
    {
        readonly string checkName;

        // This is a positional argument
        public ProblemSolverClassAttribute(string checkName)
        {
            this.checkName = checkName;

        }

        public string PositionalString
        {
            get { return checkName; }
        }

        /// <summary>
        /// Name of this check as it should appear on UI.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
