using MyPhotoshop.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MyPhotoshop
{
    public class StaticParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private static PropertyInfo[] propertyInfo;

        private static ParameterInfo[] descriptionInfo;


        public StaticParametersHandler()
        {
            propertyInfo = typeof(TParameters)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            descriptionInfo = typeof(TParameters)
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();

            var properties = propertyInfo;

            if (properties.Length != values.Length)
                throw new ArgumentException();

            for (var i = 0; i < values.Length; i++)
                properties[i].SetValue(parameters, values[i], new object[0]);

            return parameters;
        }

        public ParameterInfo[] GetDescription()
        {
            return descriptionInfo;
        }
    }
}
