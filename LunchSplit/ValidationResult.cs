using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Reports whether validation passed and explains a failure.
    /// </summary>
    /// <param name="IsValid">Whether the checked inputs are valid.</param>
    /// <param name="ErrorMessage">The failure message, or an empty string on success.</param>
    public record ValidationResult(bool IsValid, string ErrorMessage);
}
