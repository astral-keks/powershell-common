using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;

namespace AstralKeks.PowerShell.Common.Parameters
{
    public class ParameterCompleter : IArgumentCompleter
    {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName, string wordToComplete, 
            CommandAst commandAst, IDictionary fakeBoundParameters)
        {
            var source = ParameterCompleterSource.Resolve(commandName);
            if (source != null)
            {
                var callback = ParameterCompleterCallback.Resolve(source, parameterName);
                if (callback != null)
                {
                    var binding = new ParameterBinding(source);
                    binding.BindParameters(fakeBoundParameters);

                    var options = callback.Invoke(wordToComplete)
                        .Select(r => new
                        {
                            Value = r,
                            Index = r.IndexOf(wordToComplete, StringComparison.OrdinalIgnoreCase)
                        })
                        .Where(t => t.Index >= 0)
                        .GroupBy(t => t.Index)
                        .OrderBy(g => g.Key)
                        .SelectMany(g => g.Select(t => t.Value).OrderBy(v => v));
                    foreach (var option in options)
                    {
                        yield return new CompletionResult(option);
                    }
                }
            }
        }
    }
}
