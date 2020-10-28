using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T>
	{
		private PropertyInfo[] properties;

		public ValueType()
        {
			properties = GetType().GetProperties();
        }

		public bool Equals(T other)
		{
			if (other == null) return false;

			var otherProperties = other.GetType().GetProperties();

			for (var i = 0; i < properties.Length; i++)
			{
				var thisProperty = properties[i].GetValue(this);
				var otherProperty = otherProperties[i].GetValue(other);

				if (thisProperty == otherProperty && thisProperty == null)
					continue;
				if (!thisProperty.Equals(otherProperty))
					return false;
			}

			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((T)obj);
		}

		public override int GetHashCode()
		{
			unchecked
            {
				string result = "";
				for (var i = 0; i < properties.Length; i++)
                {
					var propertyData = properties[i].GetValue(this).GetHashCode().ToString();
					var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(propertyData));
					result += Convert.ToBase64String(hash);
                }
				return result.GetHashCode();
			}
		}

		public override string ToString()
		{
			var properties = GetType().GetProperties(BindingFlags.FlattenHierarchy
				| BindingFlags.Public
				| BindingFlags.Instance).OrderBy(p => p.Name).ToList();
			var result = new StringBuilder();
			result.Append(GetType().Name + "(");

			for (var i = 0; i < properties.Count; i++)
            {
				var value = properties[i].GetValue(this);

				result.Append(properties[i].Name + ": " + (value == null ? "" : value.ToString()));

				if (i < properties.Count - 1)
					result.Append("; ");
            }

			result.Append(")");

			return result.ToString();
		}
	}
}