using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RxSandbox
{
    public static class ReflectionHelper
    {
        public static MethodInfo GetMethod(Expression<Action> expression)
        {
            if (expression.Body is MethodCallExpression)
                return ((MethodCallExpression) expression.Body).Method;
            return null;
        }
    }
}