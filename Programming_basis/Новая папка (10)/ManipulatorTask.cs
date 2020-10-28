using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {

        /// <summary> 
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному angle (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double angle)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            var pointLength = Math.Sqrt(x * x + y * y);
            if (pointLength > Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm) return new double[] {double.NaN, double.NaN, double.NaN};// Проверить, не лежит ли точка дальше, чем максимальная длина манипулятора
            return new[] { double.NaN, double.NaN, double.NaN };
            // Я точно знаю позицию крайнего сустава(есть длина и угол)
            // Я смогу через теорему синусов найти угол elbow, а через него надо как-то найти угол shoulder
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            Assert.Fail("Write real tests here!");
        }
    }
}