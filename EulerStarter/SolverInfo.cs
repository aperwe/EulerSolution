using System;

namespace EulerStarter
{
    /// <summary>
    /// This class is a description of a Euler problem-solver class with metadata loaded from assembly scan.
    /// </summary>
    internal class SolverInfo
    {
        /// <summary>
        /// This is a display name presented by the solver class.
        /// </summary>
        public string DisplayName { get; internal set; }
        /// <summary>
        /// Type information about the solver class, loaded from solver-containing assembly.
        /// </summary>
        public Type TypeInfo { get; internal set; }
        /// <summary>
        /// Description of the problem this solver is trying to solve. Provided by the solver class.
        /// </summary>
        public string ProblemDefinition { get; internal set; }
    }
}