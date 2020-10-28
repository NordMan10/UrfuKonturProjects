using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PhotoshopWithoutReflection.Filters
{
    public interface ITransformer<TParameters>
        where TParameters : IParameters
    {
        void Prepare(Size oldSize, TParameters parameters);

        Size ResultSize { get; }

        Point? MapPoint(Point newPoint);
    }
}
