﻿using System;

namespace AstralKeks.PowerShell.Common.Attributes
{
    public class ValidateDynamicSetAttribute : Attribute
    {
        public ValidateDynamicSetAttribute(string valuesFunctionName)
        {
            if (string.IsNullOrWhiteSpace(valuesFunctionName))
                throw new ArgumentNullException(nameof(valuesFunctionName));

            ValuesFunctionName = valuesFunctionName;
        }

        public string ValuesFunctionName { get; set; }
    }
}
