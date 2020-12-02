using Microsoft.VisualStudio.TestTools.UnitTesting;
using QBits.Intuition.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBits.Intuition.Mathematics.Tests
{
    [TestClass()]
    public class BigIntegerTests
    {
        [TestMethod("Empty constructor test")]
        public void BigIntegerConstructorTest0()
        {
            BigInteger bigInteger = new BigInteger();
            Assert.AreEqual("0", bigInteger.ToString());
        }

        [TestMethod("Constructor test with 0 as param")]
        public void BigIntegerConstructorTest1()
        {
            BigInteger bigInteger = new BigInteger(0);
            Assert.AreEqual("0", bigInteger.ToString());
        }
        [TestMethod("Constructor test with 1 as param")]
        public void BigIntegerConstructorTest2()
        {
            BigInteger bigInteger = new BigInteger(1);
            Assert.AreEqual("1", bigInteger.ToString());
        }
        [TestMethod("Constructor test with 7 as param")]
        public void BigIntegerConstructorTest3()
        {
            BigInteger bigInteger = new BigInteger(7);
            Assert.IsTrue(bigInteger.IsPositive);
            Assert.IsFalse(bigInteger.IsNegative);
            Assert.IsFalse(bigInteger.IsEven);
            Assert.AreEqual("7", bigInteger.ToString());
        }
        [TestMethod("Constructor test with -1 as param")]
        public void BigIntegerConstructorTest4()
        {
            BigInteger bigInteger = new BigInteger(-1);
            Assert.IsFalse(bigInteger.IsPositive);
            Assert.IsTrue(bigInteger.IsNegative);
            Assert.IsFalse(bigInteger.IsEven);
            Assert.AreEqual("-1", bigInteger.ToString());
        }
        [TestMethod("Constructor test with -14 as param")]
        public void BigIntegerConstructorTest5()
        {
            BigInteger bigInteger = new BigInteger(-14);
            Assert.IsFalse(bigInteger.IsPositive);
            Assert.IsTrue(bigInteger.IsNegative);
            Assert.IsTrue(bigInteger.IsEven);
            Assert.AreEqual("-14", bigInteger.ToString());
        }
        [TestMethod("Test copy constructor")]
        public void BigIntegerConstructorTest6()
        {
            BigInteger bigInteger = new BigInteger(1);
            var copy = -bigInteger;
            Assert.AreEqual(-1, copy);
        }
        [TestMethod("Test of negation operator")]
        public void BigIntegerNegationOperatorTest()
        {
            BigInteger bigInteger = new BigInteger(7);
            var negative = -bigInteger;
            var str = negative.ToString();
            Assert.AreEqual("-7", str);
            BigInteger secondValue = -7;
            Assert.AreEqual("-7", secondValue.ToString());
        }
    }
}