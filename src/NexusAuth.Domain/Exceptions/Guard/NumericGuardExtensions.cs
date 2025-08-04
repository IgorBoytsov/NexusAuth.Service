namespace NexusAuth.Domain.Exceptions.Guard
{
    public static class NumericGuardExtensions
    {
        /// <summary>
        /// Проверяет, является ли число отрицательным.
        /// </summary>
        public static void Negative<T>(this IGuardClause guardClause, T input, string parameterName, string? message = null) where T : struct, IComparable<T>
        {
            if (input.CompareTo(default) < 0)
                throw new ArgumentOutOfRangeException(parameterName, message ?? $"Параметр {parameterName} не может быть отрицательным.");
        }

        /// <summary>
        /// Проверяет, является ли число отрицательным или нулем.
        /// </summary>
        public static void NegativeOrZero<T>(this IGuardClause guardClause, T input, string parameterName, string? message = null) where T : struct, IComparable<T>
        {
            if (input.CompareTo(default) <= 0)
                throw new ArgumentOutOfRangeException(parameterName, message ?? $"Параметр {parameterName} должен быть положительным.");
        }
    }
}