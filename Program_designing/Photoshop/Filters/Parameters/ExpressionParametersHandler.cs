using MyPhotoshop.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MyPhotoshop
{
    public class ExpressionParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private static ParameterInfo[] descriptionInfo;

        private static Func<double[], TParameters> parser;

        public ExpressionParametersHandler()
        {
            descriptionInfo = typeof(TParameters)
                .GetProperties()
                .Select(p => p.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(p => p.Length > 0)
                .Select(p => p[0])
                .Cast<ParameterInfo>()
                .ToArray();

            // values => new LightningParameters() { Coefficient = value[0] };

            var arg = Expression.Parameter(typeof(double[]), "values");

            var properties = typeof(TParameters)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
                .ToArray();

            var bindings = new List<MemberBinding>();

            for (var i = 0; i < properties.Length; i++)
            {
                var binding = Expression.Bind(
                    properties[i],
                    Expression.ArrayIndex(arg, Expression.Constant(i))
                    );
                bindings.Add(binding);
            }

            var body = Expression.MemberInit(
                Expression.New(typeof(TParameters).GetConstructor(new Type[0])),
                bindings
                );

            var lambda = Expression.Lambda<Func<double[], TParameters>>(
                body,
                arg
                );

            parser = lambda.Compile();
        }

        public TParameters CreateParameters(double[] values)
        {
            return parser(values);
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
