using PhotoshopWithoutReflection.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoshopWithoutReflection
{
    public static class ParametersExtensions
    {
        public static ParameterInfo[] GetDesсription(this IParameters parameters)
        {
            return parameters
                .GetType()
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();
                
        }

        public static void SetValues(this IParameters parameters, double[] values)
        {
            var properties = parameters
                .GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            for (var i = 0; i < values.Length; i++)
                properties[i].SetValue(parameters, values[i], new object[0]);
        }
    }
}
