using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using System.Collections.Generic;
using System.Management.Automation;

namespace AstralKeks.PowerShell.Sandbox
{
    [Cmdlet(VerbsDiagnostic.Test, "Command", SupportsShouldProcess = true)]
    public class TestCommand : Cmdlet
    {
        [Parameter]
        public List<string> One { get; set; }

        [Parameter]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public List<string> Two { get; set; }

        protected override void ProcessRecord()
        {
            
        }

        [ParameterCompleter(nameof(Two))]
        private IEnumerable<string> MyMethod(string word)
        {
            yield return $"{One}-asd";
        }

    }
}
