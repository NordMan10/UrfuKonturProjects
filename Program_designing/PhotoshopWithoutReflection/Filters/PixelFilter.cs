

using System;

namespace PhotoshopWithoutReflection.Filters
{
    public class PixelFilter<TParameters> : ParametrizedFilter<TParameters>
        where TParameters : IParameters, new()
    {
        public PixelFilter(string name, Func<Pixel, TParameters, Pixel> processor)
        {
            this.name = name;
            this.processor = processor;
        }

        string name;
        Func<Pixel, TParameters, Pixel> processor;

        public override Photo Process(Photo original, TParameters parameters)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
                for (var y = 0; y < result.Height; y++)
                {
                    result[x, y] = processor(original[x, y], parameters);
                }
            return result;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
