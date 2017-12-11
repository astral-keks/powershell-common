using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.PowerShell.Common.Extensions
{
    public static class PathIntrinsicsExtension
    {
        public static Collection<string> GetResolvedProviderPathFromPSPath(this PathIntrinsics pathIntrinsics, string path)
        {
            var paths = pathIntrinsics.GetResolvedProviderPathFromPSPath(path, out ProviderInfo provider)
                .Select(p => p.TrimEnd(Path.DirectorySeparatorChar))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return new Collection<string>(paths);
        }
    }
}
