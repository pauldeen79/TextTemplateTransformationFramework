using System;
using System.Diagnostics;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class VersionCommand : ICommandLineCommand
    {
        public void Initialize(CommandLineApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.VersionOptionFromAssemblyAttributes(GetType().Assembly);
//            app.Command("--version", command =>
//            {
//                command.Description = "Shows version information";

//#if DEBUG
//                var debuggerOption = command.Option<string>("-d|--launchdebugger", "Launches debugger", CommandOptionType.NoValue);
//#endif

//                command.HelpOption();
//                command.OnExecute(() =>
//                {
//#if DEBUG
//                    if (debuggerOption.HasValue())
//                    {
//                        Debugger.Launch();
//                    }
//#endif
//                    var versionString = Assembly.GetEntryAssembly()
//                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
//                        .InformationalVersion
//                        .ToString();

//                    app.Out.WriteLine(versionString);
//                });
//            });
        }
    }
}
