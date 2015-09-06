using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MGR.CommandLineParser.UnitTests
{
    public static class TypeHelpers
    {
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException();
            }
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException();
            }
            if (propertyInfo.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException();
            }
            return memberExpression.Member.Name;
        }
    }
}