using System.Management.Automation;

namespace AstralKeks.PowerShell.Common.Parameters
{
    public class DynamicParameterContainer : RuntimeDefinedParameterDictionary
    {
        public RuntimeDefinedParameter Get(string parameterName)
        {
            return ContainsKey(parameterName) ? this[parameterName] : null;
        }

        public TParameter GetValue<TParameter>(string parameterName)
        {
            var value = ContainsKey(parameterName) ? this[parameterName]?.Value : null;
            return value != null ? (TParameter)value : default(TParameter);
        }
    }
}
