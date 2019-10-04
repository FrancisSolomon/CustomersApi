using System.Collections.Generic;
using System.Linq;

using FluentValidation;

namespace CustomersApi.Extensions
{
    public static class ValidationExtensions
    {
        public static Dictionary<string, IEnumerable<string>> GroupedByProperty(this ValidationException exception) =>
            exception.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, v => v.Select(e => e.ErrorMessage));
    }
}
