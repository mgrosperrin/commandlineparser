using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MGR.CommandLineParser.UnitTests
{
    public static class TypeHelpers
    {
        public static string ExtractPropertyName<T, U>(Expression<Func<T, U>> propertyExpression)
            => ExtractPropertyName(propertyExpression.Body as MemberExpression);

        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
            => ExtractPropertyName(propertyExpression.Body as MemberExpression);

        private static string ExtractPropertyName(MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                throw new ArgumentNullException(nameof(memberExpression));
            }
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new InvalidOperationException("The provided expression should be aa property accession.");
            }
            if (propertyInfo.GetGetMethod(true).IsStatic)
            {
                throw new InvalidOperationException("The provided expression should be an instance property accession.");
            }
            return memberExpression.Member.Name;
        }
    }
}