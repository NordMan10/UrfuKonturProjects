using System;
using NUnit.Framework;

namespace Manipulation
{
    public class TriangleTask
    {
        /// <summary>
        /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
        /// </summary>
        public static double GetABAngle(double a, double b, double c)
        {
            if (c == 0) return 0;
            else if (a * b == 0) return double.NaN;
            var max = Math.Max(Math.Max(a, b), Math.Max(b, c));
            if (a + b + c - max > max)
                return Math.Acos((c * c - a * a - b * b) / (-2 * a * b));
            else return double.NaN;
        }
    }

    [TestFixture]
    public class TriangleTask_Tests
    {   
        [TestCase(3, 4, 5, Math.PI / 2)]
        [TestCase(1, 1, 1, Math.PI / 3)]
        [TestCase(2, 1, 2, 1.31811607165)]
        [TestCase(0, 1, 1, double.NaN)]
        [TestCase(1, 3, 1, double.NaN)]
        public void TestGetABAngle(double a, double b, double c, double expectedAngle)
        {
            Assert.AreEqual(expectedAngle, TriangleTask.GetABAngle(a, b, c));
        }
    }

}