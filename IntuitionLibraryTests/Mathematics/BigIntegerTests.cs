﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        }

        [TestMethod("Constructor test with 0 as param")]
        public void BigIntegerConstructorTest1()
        {
            BigInteger bigInteger = new BigInteger(0);
        }
        [TestMethod("Constructor test with 1 as param")]
        public void BigIntegerConstructorTest2()
        {
            BigInteger bigInteger = new BigInteger(1);
        }
        [TestMethod("Constructor test with 7 as param")]
        public void BigIntegerConstructorTest3()
        {
            BigInteger bigInteger = new BigInteger(7);
            Assert.IsTrue(bigInteger.IsPositive);
            Assert.IsFalse(bigInteger.IsNegative);
            Assert.IsFalse(bigInteger.IsEven);
        }
        [TestMethod("Constructor test with -1 as param")]
        public void BigIntegerConstructorTest4()
        {
            BigInteger bigInteger = new BigInteger(-1);
            Assert.IsFalse(bigInteger.IsPositive);
            Assert.IsTrue(bigInteger.IsNegative);
            Assert.IsFalse(bigInteger.IsEven);
        }
    }
}