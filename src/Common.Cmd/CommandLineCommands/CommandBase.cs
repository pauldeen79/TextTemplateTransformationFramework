using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    internal static class CommandBase
    {
        internal static CommandOption<string> GetCurrentDirectoryOption(CommandLineApplication command)
        {
#if !NETFRAMEWORK
            return command.Option<string>("-u|--use", "Use different current directory", CommandOptionType.SingleValue);
#else
            return null;
#endif
        }

        internal static AssemblyLoadContext CreateAssemblyLoadContext(IAssemblyService assemblyService, string assemblyName, bool currentDirectoryIsFilled, string currentDirectory)
        {
            AssemblyLoadContext assemblyLoadContext;
#if NETFRAMEWORK
            assemblyLoadContext = AssemblyLoadContext.Default;
#else
            assemblyLoadContext = new CustomAssemblyLoadContext("T4PlusCmd", true, () => currentDirectoryIsFilled
                ? new[] { currentDirectory }
                : assemblyService.GetCustomPaths(assemblyName));
#endif
            return assemblyLoadContext;
        }

        internal static string GetValidationResult(IFileContentsProvider fileContentsProvider, string filename, string assemblyName, string className)
        {
            if (string.IsNullOrEmpty(filename) && string.IsNullOrEmpty(assemblyName))
            {
                return "Either Filename or AssemblyName is required.";
            }

            if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(assemblyName))
            {
                return "You can either use Filename or AssemblyName, not both.";
            }

            if (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(className))
            {
                return "When AssemblyName is filled, then ClassName is required.";
            }

            if (!string.IsNullOrEmpty(filename) && !fileContentsProvider.FileExists(filename))
            {
                return $"File [{filename}] does not exist.";
            }

            return null;
        }
    }
}
