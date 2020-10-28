using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Differentiation
{
    public static class Algebra
    { 
        public static Expression<Func<double, double>> Differ(Expression<Func<double, double>> function)
        {
            var lambda = Expression.Lambda(Differentiate(function.Body), function.Parameters);
            return (Expression<Func<double, double>>)lambda;
        }

        private static Expression Differentiate(Expression body)
        {
            if (body is ConstantExpression)
                return Expression.Constant(0.0, typeof(double));
            if (body is ParameterExpression)
                return Expression.Constant(1.0, typeof(double));
            if (body is BinaryExpression binaryExpr)
            {
                var left = binaryExpr.Left;
                var right = binaryExpr.Right;
                switch (body.NodeType)
                {
                    case ExpressionType.Add:
                        return Expression.Add(Differentiate(left), Differentiate(right));
                    case ExpressionType.Subtract:
                        return Expression.Add(Differentiate(left), Differentiate(right));
                    case ExpressionType.Multiply:
                        return Expression.Add(
                        Expression.Multiply(left, Differentiate(right)),
                        Expression.Multiply(Differentiate(left), right));
                    case ExpressionType.Divide:
                        return Expression.Divide(
                        Expression.Subtract(
                        Expression.Multiply(left, Differentiate(right)),
                        Expression.Multiply(Differentiate(left), right)),
                        Expression.Multiply(right, right));
                }
            }
            if (body is MethodCallExpression methodCallexpr)
            {
                MethodInfo method;
                double constant = 1;
                switch (methodCallexpr.Method.Name)
                {
                    case "Sin":
                        method = typeof(Math).GetMethod("Cos");
                        break;
                    case "Cos":
                        method = typeof(Math).GetMethod("Sin");
                        constant = -1;
                        break;
                    default:
                        throw new Exception();
                }
                return Expression.Multiply(
                Differentiate(methodCallexpr.Arguments.First()),
                Expression.Multiply(
                Expression.Constant(constant, typeof(double)),
                Expression.Call(null, method, methodCallexpr.Arguments)));
            }

            throw new Exception("Не обработан тип выражения: " + body.NodeType.ToString());
        }
    }
}