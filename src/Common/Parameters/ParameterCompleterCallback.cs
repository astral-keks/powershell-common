using AstralKeks.PowerShell.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AstralKeks.PowerShell.Common.Parameters
{
    internal class ParameterCompleterCallback
    {
        private readonly Func<string, IEnumerable<string>> _method;

        public ParameterCompleterCallback(Func<string, IEnumerable<string>> completer)
        {
            _method = completer ?? throw new ArgumentNullException(nameof(completer));
        }

        public Func<string, IEnumerable<string>> Invoke => _method;

        public static ParameterCompleterCallback Resolve(ParameterCompleterSource source, string parameterName)
        {
            Delegate completer = null;

            if (source != null)
            {
                var methodInfo = source.Methods.FirstOrDefault(m => IsCompleter(m, parameterName));

                try
                {
                    completer = Delegate.CreateDelegate(typeof(Func<string, IEnumerable<string>>), source.Cmdlet, methodInfo);
                }
                catch (ArgumentException)
                {
                    completer = null;
                }
            }

            return completer != null && completer is Func<string, IEnumerable<string>> 
                ? new ParameterCompleterCallback(completer as Func<string, IEnumerable<string>>)
                : null;
        }

        private static bool IsCompleter(MethodInfo method, string parameterName)
        {
            var parameterNames = method.GetCustomAttribute<ParameterCompleterAttribute>()?.ParameterNames;
            return parameterNames?.Any(p => string.Equals(p, parameterName, StringComparison.OrdinalIgnoreCase)) == true;
        }
    }
}
