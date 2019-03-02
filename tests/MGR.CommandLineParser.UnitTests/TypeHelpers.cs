using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MGR.CommandLineParser.UnitTests
{
    public static class TypeHelpers
    {
        public static string ExtractPropertyName<T, U>(Expression<Func<T, U>> propertyExpression)
            => ExtractPropertyName(propertyExpression.Body as MemberExpression);

        public static string ExtractPropertyName<T>(Expression<Func<T, object>> propertyExpression)
            => ExtractPropertyName(propertyExpression.Body as MemberExpression);

        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
            => ExtractPropertyName(propertyExpression.Body as MemberExpression);

        private static string ExtractPropertyName(MemberExpression memberExpression)
        {
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