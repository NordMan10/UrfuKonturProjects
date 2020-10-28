using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask 
    {
        //public static float[] GetCoordinates(double angle, float length)
        //{
        //    var coordinates = new float[2];
        //    coordinates[0] = length * (float)Math.Cos(angle);
        //    coordinates[1] = length * (float)Math.Sin(angle);
        //    return coordinates;     
        //}

        /// <summary>
        /// По значению углов суставов возвращает массив координат суставов
        /// в порядке new []{elbow, wrist, palmEnd}
        /// </summary>
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var wristAngle = shoulder + Math.PI + elbow;
            var palmAngle = shoulder + Math.PI + elbow + Math.PI + wrist;
            var elbowPos = new PointF(Manipulator.UpperArm * (float)Math.Cos(shoulder), Manipulator.UpperArm * (float)Math.Sin(shoulder));
            var wristPos = new PointF(Manipulator.Forearm * (float) Math.Cos(wristAngle) + elbowPos.X, Manipulator.Forearm * (float)Math.Sin(wristAngle) + elbowPos.Y);
            var palmEndPos = new PointF(Manipulator.Palm * (float)Math.Cos(palmAngle) + wristPos.X, Manipulator.Palm * (float)Math.Sin(palmAngle) + wristPos.Y);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }
    }
    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        // Доработайте эти тесты!
        // С помощью строчки TestCase можно добавлять новые тестовые данные.
        // Аргументы TestCase превратятся в аргументы метода.
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm, Manipulator.Forearm, Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY, double forearmDist, double palmDist)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            var distance1 = Math.Sqrt(joints[0].X * joints[0].X + joints[0].Y * joints[0].Y);
            var distance2 = Math.Round(Math.Sqrt(Math.Pow(joints[1].X - joints[0].X, 2) + Math.Pow(joints[1].Y - joints[0].Y, 2)));
            var distance3 = Math.Sqrt(Math.Pow(joints[2].X - joints[1].X, 2) + Math.Pow(joints[2].Y - joints[1].Y, 2));
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
            Assert.AreEqual(palmEndY, distance1);
            Assert.AreEqual(forearmDist, distance2);
            Assert.AreEqual(palmDist, distance3);
            Assert.Fail("TODO: проверить, что расстояния между суставами равны длинам сегментов манипулятора!");
        }   

    }

}