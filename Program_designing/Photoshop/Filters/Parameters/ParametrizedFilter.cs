using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoshop.Filters
{
    public abstract class ParametrizedFilter<TParameters> : IFilter
        where TParameters : IParameters, new()
    {
        public IParametersHandler<TParameters> handler = new ExpressionParametersHandler<TParameters>();

        public ParameterInfo[] GetParameters()
        {
            return handler.GetDescription();
        }

        public Photo Process(Photo original, double[] values)
        {
            return Process(original, handler.CreateParameters(values));
        }

        public abstract Photo Process(Photo original, TParameters parameters);
    }
}
