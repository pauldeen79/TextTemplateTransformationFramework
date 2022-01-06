using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;

namespace TextTemplateTransformationFramework.Common.Cmd.Extensions
{
    public static class CommandOptionOfStringExtensions
    {
        public static void LaunchDebuggerIfSet(this CommandOption<string> debuggerOption)
        {
            if (debuggerOption.HasValue())
            {
                Debugger.Launch();
            }
        }
    }
}
