using System;

namespace AstralKeks.PowerShell.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParameterCompleterAttribute : Attribute
    {
        public ParameterCompleterAttribute(params string[] parameterNames)
        {
            ParameterNames = parameterNames;
        }

        public string[] ParameterNames { get; set; }
    }
}
