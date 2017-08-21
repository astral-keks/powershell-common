using System;

namespace AstralKeks.PowerShell.Common.Attributes
{
    public class DynamicCompleterAttribute : Attribute
    {
        public DynamicCompleterAttribute(string completerFunctionName)
        {
            if (string.IsNullOrWhiteSpace(completerFunctionName))
                throw new ArgumentNullException(nameof(completerFunctionName));

            CompleterFunctionName = completerFunctionName;
        }

        public string CompleterFunctionName { get; set; }
    }
}
