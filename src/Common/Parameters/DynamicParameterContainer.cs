﻿using System.Management.Automation;

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
            return ContainsKey(parameterName) ? (TParameter)this[parameterName]?.Value : default(TParameter);
        }
    }
}