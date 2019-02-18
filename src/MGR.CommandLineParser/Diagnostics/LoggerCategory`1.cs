using System;
using JetBrains.Annotations;

namespace MGR.CommandLineParser.Diagnostics
{
    internal abstract class LoggerCategory<T>
    {
        public static string Name { get; } = ToName(typeof(T));

        public override string ToString() => Name;

        public static implicit operator string([NotNull] LoggerCategory<T> loggerCategory) => loggerCategory.ToString();

        private static string ToName([NotNull] Type loggerCategoryType)
        {
            const string outerClassName = "." + nameof(LoggerCategory);

            var name = loggerCategoryType.FullName.Replace('+', '.');
            var index = name.IndexOf(outerClassName, StringComparison.Ordinal);
            if (index >= 0)
            {
                name = name.Substring(0, index) + name.Substring(index + outerClassName.Length);
            }

            return name;
        }
    }
}
