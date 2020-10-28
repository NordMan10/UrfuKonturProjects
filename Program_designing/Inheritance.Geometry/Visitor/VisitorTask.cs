
using System.Collections.Generic;

namespace Inheritance.Geometry.Visitor
{
    public abstract class Body
    {
        public Vector3 Position { get; }

        protected Body(Vector3 position)
        {
            Position = position;
        }

        public abstract void Accept(IVisitor visitor);
    }

    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3 position, double radius) : base(position)
        {
            Radius = radius;
        }

        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }

    public class RectangularCuboid : Body
    {
        public double SizeX { get; }
        public double SizeY { get; }
        public double SizeZ { get; }

        public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
        }

        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Cylinder : Body
    {
        public double SizeZ { get; }

        public double Radius { get; }

        public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
        {
            SizeZ = sizeZ;
            Radius = radius;
        }

        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CompoundBody : Body
    {
        public IReadOnlyList<Body> Parts { get; }

        public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
        {
            Parts = parts;
        }

        public override void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }

    public class BoundingBoxVisitor : IVisitor
    {
        public Body VisitBall(Ball ball)
        {
            return new RectangularCuboid(ball.Position, 2 * ball.Radius, 2 * ball.Radius, 2 * ball.Radius);
        }

        public Body VisitCylinder(Cylinder cylinder)
        {
            return new RectangularCuboid(cylinder.Position, 2 * cylinder.Radius, 2 * cylinder.Radius, cylinder.SizeZ);
        }

        public Body VisitRectangularCuboid(RectangularCuboid rectCuboid)
        {
            return new RectangularCuboid(rectCuboid.Position, rectCuboid.SizeX, rectCuboid.SizeY, rectCuboid.SizeZ);
        }

        public Body VisitCompoundBody(CompoundBody compoundBody)
        {
            var minVectors = new List<Vector3>();
            var maxVectors = new List<Vector3>();

            SetMinAndMaxVectors(compoundBody.Parts, minVectors, maxVectors);

            var mostMinPoint = GetMinVector(minVectors);
            var mostMaxPoint = GetMaxlVector(maxVectors);

            var sizeX = mostMaxPoint.X - mostMinPoint.X;
            var sizeY = mostMaxPoint.Y - mostMinPoint.Y;
            var sizeZ = mostMaxPoint.Z - mostMinPoint.Z;

            var position = new Vector3(mostMinPoint.X + sizeX / 2,
                mostMinPoint.Y + sizeY / 2, mostMinPoint.Z + sizeZ / 2);

            return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
        }

        private Vector3 GetMinVector(List<Vector3> minVectors)
        {
            double x = double.MaxValue;
            double y = x;
            double z = x;

            for (var i = 0; i < minVectors.Count - 1; i++)
            {
                x = Math.Min(minVectors[i].X, minVectors[i + 1].X) < x ?
                    Math.Min(minVectors[i].X, minVectors[i + 1].X) : x;
                y = Math.Min(minVectors[i].Y, minVectors[i + 1].Y) < y ?
                    Math.Min(minVectors[i].Y, minVectors[i + 1].Y) : y;
                z = Math.Min(minVectors[i].Z, minVectors[i + 1].Z) < z ?
                    Math.Min(minVectors[i].Z, minVectors[i + 1].Z) : z;
            }

            return new Vector3(x, y, z);
        }

        private Vector3 GetMaxlVector(List<Vector3> maxVectors)
        {
            double x = double.MinValue;
            double y = x;
            double z = x;

            for (var i = 0; i < maxVectors.Count - 1; i++)
            {
                x = Math.Max(maxVectors[i].X, maxVectors[i + 1].X) > x ?
                    Math.Max(maxVectors[i].X, maxVectors[i + 1].X) : x;
                y = Math.Max(maxVectors[i].Y, maxVectors[i + 1].Y) > y ?
                    Math.Max(maxVectors[i].Y, maxVectors[i + 1].Y) : y;
                z = Math.Max(maxVectors[i].Z, maxVectors[i + 1].Z) > z ?
                    Math.Max(maxVectors[i].Z, maxVectors[i + 1].Z) : z;
            }

            return new Vector3(x, y, z);
        }

        private void SetMinAndMaxVectors(IReadOnlyList<Body> parts, List<Vector3> minVectors, List<Vector3> maxVectors)
        {
            foreach (var body in parts)
            {
                var boundingBox = body.Accept()    
                var minPoint = new Vector3(
                    boundingBox.Position.X - boundingBox.SizeX / 2,
                    boundingBox.Position.Y - boundingBox.SizeY / 2,
                    boundingBox.Position.Z - boundingBox.SizeZ / 2);
                var maxPoint = new Vector3(
                    boundingBox.Position.X + boundingBox.SizeX / 2,
                    boundingBox.Position.Y + boundingBox.SizeY / 2,
                    boundingBox.Position.Z + boundingBox.SizeZ / 2);

                minVectors.Add(minPoint);
                maxVectors.Add(maxPoint);
            }
        }
    }

    public class BoxifyVisitor
    {
        //TODO
    }
}