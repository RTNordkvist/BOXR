using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BOXR.UI.ValidationRules
{
    public class NotNullRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "Please fill out the field");
            }

            return ValidationResult.ValidResult;
        }
    }
}
