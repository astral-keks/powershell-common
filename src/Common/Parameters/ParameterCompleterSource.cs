using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AstralKeks.PowerShell.Common.Parameters
{
    internal class ParameterCompleterSource
    {
        private readonly Cmdlet _cmdlet;
        private readonly object _dynamicParameters;

        public ParameterCompleterSource(Cmdlet cmdlet)
        {
            _cmdlet = cmdlet ?? throw new ArgumentNullException(nameof(cmdlet));
            _dynamicParameters = (_cmdlet as IDynamicParameters)?.GetDynamicParameters();
        }

        public Cmdlet Cmdlet => _cmdlet;

        public PropertyInfo[] Properties => _cmdlet.GetType().GetProperties();

        public MethodInfo[] Methods => _cmdlet.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        public object DynamicParameters => _dynamicParameters;

        public RuntimeDefinedParameterDictionary DynamicDictionary => _dynamicParameters as RuntimeDefinedParameterDictionary;

        public PropertyInfo[] DynamicProperties => _dynamicParameters?.GetType().GetProperties();

        public static ParameterCompleterSource Resolve(string commandName)
        {
            Cmdlet cmdlet = null;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies.Where(IsModule))
            {
                var types = assembly.GetTypes();
                var cmdletType = types.FirstOrDefault(t => IsCommand(t, commandName));
                if (cmdletType != null)
                {
                    cmdlet = (Cmdlet)Activator.CreateInstance(cmdletType);
                }
            }

            return cmdlet != null ? new ParameterCompleterSource(cmdlet) : null;
        }

        private static bool IsModule(Assembly assembly)
        {
            return assembly
                .GetReferencedAssemblies()
                .Any(a => a.FullName.StartsWith("system.management.automation,", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsCommand(Type cmdletType, string commandName)
        {
            var isCmdlet = cmdletType.IsSubclassOf(typeof(Cmdlet));
            var cmdletAttribute = cmdletType.GetCustomAttribute<CmdletAttribute>();
            var cmdletCommandName = $"{cmdletAttribute?.VerbName}-{cmdletAttribute?.NounName}";
            return isCmdlet && string.Equals(commandName, cmdletCommandName, StringComparison.OrdinalIgnoreCase);
        }

    }
}
