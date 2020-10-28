using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class FromDistribution : Attribute
    {
        public FromDistribution(Type distribution, params double[] parameters)
        {
            var constrContentType = new Type[parameters.Length];
            var constrInvokeArray = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                constrContentType[i] = typeof(double);
                constrInvokeArray[i] = parameters[i];
            }

            var constructor = distribution.GetConstructor(constrContentType);
            if (constructor == null) 
                throw new ArgumentException(string.Format("Fail with type parameter {0}", distribution.Name));

            Distribution = (IContinuousDistribution)constructor.Invoke(constrInvokeArray);
        }

        public IContinuousDistribution Distribution { get; private set; }
    }

    public class Generator<T>
        where T : new()
    {
        public Dictionary<PropertyInfo, IContinuousDistribution> propertyDistrReferences { get; }

        public Generator()
        {
            propertyDistrReferences = typeof(T).GetProperties()
            .ToDictionary(p => p, p => p.GetCustomAttribute<FromDistribution>()?.Distribution);
        }

        public Generator(Dictionary<PropertyInfo, IContinuousDistribution> propertyDistrReferences)
        {
            this.propertyDistrReferences = propertyDistrReferences;
        }

        public T Generate(Random rnd)
        {
            var result = new T();

            foreach (var propertyDistrReference in propertyDistrReferences)
            {
                if (propertyDistrReference.Value != null)
                    propertyDistrReference.Key.SetValue(result, propertyDistrReference.Value.Generate(rnd));
            }

            return result;
        }
    }

    public static class GeneratorExtensions
    {
        public static Config<T> For<T>(this Generator<T> generator, Expression<Func<T, double>> handler)
            where T : new()
        {
            if (!(handler.Body is MemberExpression memberExpression) 
                || memberExpression.Member.DeclaringType != typeof(T))
                throw new ArgumentException();

            var propertyName = (handler.Body as MemberExpression).Member.Name;
            return new Config<T>(generator.propertyDistrReferences, 
                generator.propertyDistrReferences.First(p => p.Key.Name == propertyName).Key);
        }

        public static Generator<T> Set<T>(this Config<T> config, IContinuousDistribution distribution)
            where T : new()
        {
            config.PropertyDistrReferences[config.Key] = distribution;

            return new Generator<T>(config.PropertyDistrReferences);
        }

        public class Config<T>
            where T : new()
        {
            public Config(Dictionary<PropertyInfo, IContinuousDistribution> propertyDistrReferences, PropertyInfo key)
            {
                PropertyDistrReferences = propertyDistrReferences;
                Key = key;
            }

            public Dictionary<PropertyInfo, IContinuousDistribution> PropertyDistrReferences { get; set; }
            public PropertyInfo Key { get; }
        }
    }
}
