using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Mathematics
{
    /*
     *    No matter where you got this code, be aware that MIRACL is NOT free software. For commercial use a license is required.
     *	  See www.shamus.ie
     *
     *    MIRACL  C++ Header file big.h
     *    AUTHOR  :    N.Coghlan
     *                 Modified by M.Scott
     *    PURPOSE :    Definition of class Big
     *
     *   Bigs are normally created on the heap, but by defining BIGS=m on the compiler command line, Bigs are instead mostly created from the 
     *   stack. Note that m must be same or less than the n in the main program with for example 
     *
     *   Miracl precison(n,0); 
     *   where n is the (fixed) size in words of each Big.
     *
     *   This may be faster, as C++ tends to create and destroy lots of temporaries. Especially recommended if m is small. Do not use for program development
     *
     *   However Bigs created from a string are always allocated from the heap.
     *   This is useful for creating large read-only constants which are larger than m. 
     *
     *   NOTE:- I/O conversion
     *
     *   To convert a hex character string to a Big
     *         Big x;
     *         char c[100];
     *
     *         mip->IOBASE=16;
     *         x=c;
     *
     *   To convert a Big to a hex character string
     * 
     *         mip->IOBASE=16;
     *         c << x;
     *
     *   To convert to/from pure binary, see the from_binary() and to_binary() friend functions.
     *
     *   int len;
     *   char c[100];
     *   ...
     *   Big x=from_binary(len,c);  // creates Big x from len bytes of binary in c 
     *
     *   len=to_binary(x,100,c,FALSE); // converts Big x to len bytes binary in c[100] 
     *   len=to_binary(x,100,c,TRUE);  // converts Big x to len bytes binary in c[100] 
     *                                 // (right justified with leading zeros)
     *                              
     *   Copyright (c) 1988-2001 Shamus Software Ltd.
     */

    /// <summary>
    /// A real number of arbitrary precision.
    /// </summary>
    public abstract class Real : IComparable<Real>
    {
        /// <summary>
        /// Protected constructor to prevent instantiation outside the factory method.
        /// </summary>
        protected Real()
        {
        }

        #region Private fields
        private bool Negative { get; set; }
        internal RealType MyType { get; set; }
        #endregion

        #region Public API
        /// <summary>
        /// Indicates whether this number is negative or not.
        /// </summary>
        public bool IsNegative { get { return Negative; } }
        /// <summary>
        /// Factory method that produces an appropriate implementaion of <see cref="Real"/> that 
        /// is optimized for the specified input parameter (<paramref name="initialValue"/>);
        /// </summary>
        /// <param name="initialValue">Value initially stored in this <see cref="Real"/> number</param>
        public static Real CreateReal(double initialValue)
        {
            return new RealDouble(initialValue);
        }
        /// <summary>
        /// Attempts to obtain a 'double' value of this Real.
        /// </summary>
        public abstract double ToDouble();
        /// <summary>
        /// Attempts to add the other <see cref="Real"/> to this <see cref="Real"/>.
        /// </summary>
        /// <param name="other">The other <see cref="Real"/> value to be added.</param>
        /// <returns>A <see cref="Real"/> value being the sum of this and the other values.</returns>
        public abstract Real Add(Real other);
        /// <summary>
        /// Attempts to convert this <see cref="Real"/> to 'double'.
        /// </summary>
        public static implicit operator double(Real real)
        {
            return real.ToDouble();
        }
        /// <summary>
        /// Adds two Real values and returns a Real that is the sum of both.
        /// </summary>
        public static Real operator +(Real left, Real right)
        {
            return left.Add(right);
        }
        /// <summary>
        /// Comparer method.
        /// </summary>
        /// <param name="other">Element to compare.</param>
        public abstract int CompareTo(Real other);
        #endregion
    }
    /// <summary>
    /// Optimized implementation of Real that is based on a single 'double' value.
    /// </summary>
    public class RealDouble : Real
    {
        /// <summary>
        /// Constructs a Real based on 'double' value type.
        /// </summary>
        internal RealDouble(double value)
        {
            MyType = RealType.OneDouble;
            ActualValue = value;
        }
        private double ActualValue { get; set; }
        /// <summary>
        /// Returns the value of ActualValue as double.
        /// </summary>
        public override double ToDouble()
        {
            return ActualValue;
        }
        /// <summary>
        /// Adds two reals.
        /// </summary>
        public override Real Add(Real other)
        {
            if (other is RealDouble)
            {
                var real = other as RealDouble;
                return Real.CreateReal(ActualValue + real.ActualValue);
            }
            throw new System.NotImplementedException("Adding RealDouble with other Real type is not yet supported");
        }
        /// <summary>Comparison implementation.</summary>
        public override int CompareTo(Real other)
        {
            if (other is RealDouble)
            {
                var real = other as RealDouble;
                return ActualValue.CompareTo(real.ActualValue);
            }
            throw new System.NotImplementedException("Comparing RealDouble to other types is not yet supported");
        }
        public override string ToString()
        {
            return ActualValue.ToString();
        }
    }
    /// <summary>
    /// Indicates what kind of the real a <see cref="Real"/> actually is.
    /// </summary>
    internal enum RealType
    {
        /// <summary>
        /// The real is actually represented by a double.
        /// Internally the value in the real number is simply a double.
        /// </summary>
        OneDouble
    }
}
