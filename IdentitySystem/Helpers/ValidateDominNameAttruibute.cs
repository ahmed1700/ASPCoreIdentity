using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentitySystem.Helpers
{
    public class ValidateDominNameAttribute : ValidationAttribute
    {
        private readonly string allowedDominName;

        public ValidateDominNameAttribute(string _allowedDominName)
        {
            this.allowedDominName = _allowedDominName;
        }

        public override bool IsValid(object value)
        {
            string[] vals = value.ToString().Split('@');
            return vals[1].ToUpper() == allowedDominName.ToUpper();
        }
    }
}
