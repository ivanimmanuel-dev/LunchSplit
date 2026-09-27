using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    public record ValidationResult(bool IsValid, string ErrorMessage);
}
