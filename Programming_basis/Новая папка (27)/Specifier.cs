using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            var attributes = typeof(T)
            .GetCustomAttributes()
            .OfType<ApiDescriptionAttribute>();

            if (attributes.Count() == 0) return null;

            return attributes.First().Description;
        }

        public string[] GetApiMethodNames()
        {
            var methodNames = new List<string>();
            var methodList = typeof(T)
            .GetMethods()
            .Where(m => m.GetCustomAttributes()
            .Contains(new ApiMethodAttribute()));
            foreach (var method in methodList)
            {
                methodNames.Add(method.Name);
            }
            return methodNames.ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method == null) return null;
            var requiredAttributes = method.GetCustomAttributes()
            .OfType<ApiDescriptionAttribute>();
            if (requiredAttributes.Count() == 0) return null;
            return requiredAttributes.First().Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var paramsInfo = typeof(T).GetMethod(methodName).GetParameters();
            var paramsNames = paramsInfo.Select(p => p.Name).ToArray();
            return paramsNames;
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null) return null;
            var parameters = methodInfo
                .GetParameters()
                .Where(p => p.Name == paramName);
            if (parameters.Count() == 0) return null;
            var requiredAttributes = parameters.First()
                .GetCustomAttributes()
                .OfType<ApiDescriptionAttribute>();
            if (requiredAttributes.Count() == 0) return null;
            var paramDescription = requiredAttributes
                .First().Description;
            return paramDescription;
        }

        public Tuple<IEnumerable<Attribute>, ApiParamDescription> GetAndCheckParamAttributes(
            IEnumerable<ParameterInfo> parameters, ApiParamDescription result)
        {
            var paramAttributes = parameters
                .First()
                .GetCustomAttributes();
            if (paramAttributes.Contains(new ApiRequiredAttribute())) result.Required = true;
            else result.Required = false;
            return Tuple.Create(paramAttributes, result);
        }

        public ApiParamDescription SetParamDescription(
            IEnumerable<Attribute> paramAttributes,
            string paramName,
            ApiParamDescription result)
        {
            var paramAttributesDescription = paramAttributes
            .OfType<ApiDescriptionAttribute>();
            string paramDescription;
            if (paramAttributesDescription.Count() != 0)
                paramDescription = paramAttributesDescription
                .First()
                .Description;
            else paramDescription = null;
            result.ParamDescription = new CommonDescription(paramName, paramDescription);
            return result;
        }

        public ApiParamDescription SetMaxAndMinValue(
            IEnumerable<Attribute> paramAttributes,
            ApiParamDescription result)
        {
            var intValidationAttributes = paramAttributes
            .OfType<ApiIntValidationAttribute>();
            if (intValidationAttributes.Count() == 0)
            {
                result.MinValue = null;
                result.MaxValue = null;
                return result;
            }
            var firstIntValidationAttribute = intValidationAttributes.First();
            result.MaxValue = firstIntValidationAttribute.MaxValue;
            result.MinValue = firstIntValidationAttribute.MinValue;
            return result;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var result = new ApiParamDescription();
            var methodInfo = typeof(T).GetMethod(methodName);
            var defaultName = "some";
            if (methodInfo == null)
            {
                result.ParamDescription = new CommonDescription { Name = defaultName };
                return result;
            }
            var parameters = methodInfo.GetParameters()
                .Where(p => p.Name == paramName);

            if (parameters.Count() == 0)
            {
                result.ParamDescription = new CommonDescription(paramName);
                return result;
            }
            var paramAttributesAndResult = GetAndCheckParamAttributes(parameters, result);
            var paramAttributes = paramAttributesAndResult.Item1;
            result = paramAttributesAndResult.Item2;
            result = SetParamDescription(paramAttributes, paramName, result);
            result = SetMaxAndMinValue(paramAttributes, result);
            return result;
        }

        public ApiParamDescription[] SetParamsDescription(MethodInfo methodInfo,
            string methodName)
        {
            var paramsDescription = methodInfo.GetParameters()
                .Select(p => GetApiMethodParamFullDescription(methodName, p.Name)).ToArray();
            return paramsDescription;
        }

        public ApiMethodDescription SetRequireFlag(ICustomAttributeProvider returnInfo,
            ApiMethodDescription result)
        {
            var returnParamsRequired = returnInfo
                .GetCustomAttributes(true)
                .OfType<ApiRequiredAttribute>();
            if (returnParamsRequired.Count() != 0)
            {
                result.ReturnDescription = new ApiParamDescription
                {
                    Required = returnParamsRequired.First().Required
                };
            }
            return result;
        }

        public ApiMethodDescription SetReturnMaxAndMinValue(
            ICustomAttributeProvider returnInfo,
            ApiMethodDescription result)
        {
            var returnParamsIntValidation = returnInfo
            .GetCustomAttributes(true)
            .OfType<ApiIntValidationAttribute>();
            if (returnParamsIntValidation.Count() != 0)
            {
                result.ReturnDescription.MaxValue = returnParamsIntValidation.First().MaxValue;
                result.ReturnDescription.MinValue = returnParamsIntValidation.First().MinValue;
            }
            return result;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var result = new ApiMethodDescription();
            var methodInfo = typeof(T).GetMethod(methodName);
            if (methodInfo == null) return null;

            var methodAttributes = methodInfo.GetCustomAttributes().OfType<ApiMethodAttribute>();
            if (methodAttributes.Count() == 0) return null;

            result.MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName));

            result.ParamDescriptions = SetParamsDescription(methodInfo, methodName);

            var returnInfo = methodInfo.ReturnTypeCustomAttributes;
            result = SetRequireFlag(returnInfo, result);

            result = SetReturnMaxAndMinValue(returnInfo, result);
            return result;
        }
    }
}