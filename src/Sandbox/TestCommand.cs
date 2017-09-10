using AstralKeks.PowerShell.Common;
using AstralKeks.PowerShell.Common.UserInterface;
using System;
using System.Management.Automation;

namespace AstralKeks.PowerShell.Sandbox
{
    [Cmdlet(VerbsDiagnostic.Test, "Command")]
    public class TestCommand : DynamicPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var result = Host.UI.PromptForAnswer("Eh?", new[] { PromptAnswer.Yes, PromptAnswer.No });
            WriteObject(result);
        }
    }
}
