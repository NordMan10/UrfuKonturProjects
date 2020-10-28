using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyPhotoshop.Filters
{
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters>
        where TParameters : IParameters, new()
    {
        public TransformFilter(string name, ITransformer<TParameters> transformer)
        {
            this.name = name;
            this.transformer = transformer;
        }

        ITransformer<TParameters> transformer;

        string name;

        public override string ToString()
        {
            return name;
        }

        public override Photo Process(Photo original, TParameters parameters)
        {
            var oldSize = new Size(original.Width, original.Height);
            transformer.Prepare(oldSize, parameters);
            var result = new Photo(transformer.ResultSize.Width, transformer.ResultSize.Height);

            for (var x = 0; x < result.Width; x++)
                for (var y = 0; y < result.Height; y++)
                {
                    var point = new Point(x, y);
                    var oldPoint = transformer.MapPoint(point);
                    if (oldPoint.HasValue)
                        result[x, y] = original[oldPoint.Value.X, oldPoint.Value.Y];
                }

            return result;
        }
    }
}
