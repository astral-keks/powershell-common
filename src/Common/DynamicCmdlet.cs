using System.Management.Automation;
using AstralKeks.PowerShell.Common.Parameters;

namespace AstralKeks.PowerShell.Common
{
    public class DynamicCmdlet : Cmdlet, IDynamicParameters
    {
        public DynamicParameterContainer Parameters { get; } = new DynamicParameterContainer();

        public object GetDynamicParameters()
        {
            var builder = new DynamicParameterBuilder(this);
            builder.Build(Parameters);

            return Parameters;
        }
    }
}
