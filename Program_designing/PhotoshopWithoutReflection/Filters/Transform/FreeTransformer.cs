using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PhotoshopWithoutReflection.Filters.Transform
{
    public class FreeTransformer : ITransformer<EmptyParameters>
    {
        public FreeTransformer(Func<Size, Size> sizeTransformer, Func<Point, Size, Point> pointTransformer)
        {
            this.sizeTransformer = sizeTransformer;
            this.pointTransformer = pointTransformer;
        }

        public Size ResultSize { get; private set; }

        Size oldSize;

        Func<Size, Size> sizeTransformer;
        Func<Point, Size, Point> pointTransformer;

        public Point? MapPoint(Point newPoint)
        {
            return pointTransformer(newPoint, oldSize);
        }

        public void Prepare(Size oldSize, EmptyParameters parameters)
        {
            this.oldSize = oldSize;
            ResultSize = sizeTransformer(oldSize);
        }
    }
}
