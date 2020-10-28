using MyPhotoshop.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPhotoshop
{
    public class SimpleParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();
            var properties = parameters
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            for (var i = 0; i < values.Length; i++)
                properties[i].SetValue(parameters, values[i], new object[0]);

            return parameters;
        }

        public ParameterInfo[] GetDescription()
        {
            return typeof(TParameters)
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }
    }
}
