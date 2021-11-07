using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus
{
    internal static class TokenPlaceholderProcessor
    {
        internal static readonly string[] HintPathFileExtensions = new[]
        {
            ".cs",
            ".dll",
            ".tt",
            ".ttinclude",
            ".template"
        };
    }

    public class TokenPlaceholderProcessor<TState> : ITokenPlaceholderProcessor<TState>
        where TState : class
    {
        public string Process(string value, IEnumerable<ITemplateToken<TState>> existingTokens, TokenParserState state)
        {
            if (value == null)
            {
                return null;
            }

            value = ProcessVariable<ITemplateBasePathToken<TState>>
            (
                existingTokens,
                value,
                "$(BasePath)",
                t => t.BasePath,
                () => state.Parameters.NotNull().Where(p => p.Name == Constants.BasePathParameterName)
                                                .Select(p => p.Value.ToStringWithDefault())
                                                .LastOrDefault()
            );
            value = value.IndexOf("$(NuGetDir)", StringComparison.OrdinalIgnoreCase) == -1
                ? value
                : value.Replace("$(NuGetDir)", Environment.GetEnvironmentVariable("UserProfile") + @"\.nuget\packages\", StringComparison.OrdinalIgnoreCase);

            value = value.IndexOf("$(NETCoreAppDir)", StringComparison.OrdinalIgnoreCase) == -1
                ? value
                : value.Replace("$(NETCoreAppDir)", $@"{Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)}\dotnet\shared\Microsoft.NETCore.App\{Environment.Version}\", StringComparison.OrdinalIgnoreCase);

            value = value.IndexOf("$(TempPath)", StringComparison.OrdinalIgnoreCase) == -1
                ? value
                : value.Replace("$(TempPath)", Path.GetTempPath(), StringComparison.OrdinalIgnoreCase);

            if (value.IndexOf("$(TemplateParameter.", StringComparison.OrdinalIgnoreCase) > -1)
            {
                value = existingTokens
                    .GetTemplateTokensFromSections<TState, ITemplateParameterToken<TState>>()
                    .Aggregate
                    (
                        value,
                        (v, parameterToken) =>
                            v.Replace(string.Format("$(TemplateParameter.{0})", parameterToken.Name), parameterToken.Value, StringComparison.OrdinalIgnoreCase)
                    )
                    .Apply
                    (
                        v1 =>
                            state.Parameters
                                .NotNull()
                                .Aggregate
                                (
                                    v1,
                                    (v, templateParameter) =>
                                        v.Replace(string.Format("$(TemplateParameter.{0})", templateParameter.Name), templateParameter.Value.ToStringWithDefault(string.Empty), StringComparison.OrdinalIgnoreCase)
                                )
                    );
            }

            if (IsFileForHintPath(value) && !Path.IsPathRooted(value))
            {
                //relative path
                var file = existingTokens
                    .GetTemplateTokensFromSections<TState, IHintPathToken<TState>>()
                    .Where(t => string.IsNullOrEmpty(t.Name) || t.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                    .SelectMany(hintPathToken => hintPathToken.HintPath.GetDirectories(hintPathToken.Recursive).Select(p => PathSafeCombine(p, value)))
                    .FirstOrDefault(fullPath => !string.IsNullOrEmpty(fullPath) && File.Exists(fullPath));

                if (file != null)
                {
                    return file;
                }
            }

            return existingTokens
                .GetTemplateTokensFromSections<TState, IPlaceholderProcessorToken<TState>>()
                .Aggregate
                (
                    value,
                    (v, processPlaceholderProcessorToken) => processPlaceholderProcessorToken.Process(v, existingTokens)
                );
        }

        private static bool IsFileForHintPath(string value)
            => value.EndsWithAny(StringComparison.OrdinalIgnoreCase, TokenPlaceholderProcessor.HintPathFileExtensions);

        private string ProcessVariable<TValue>
        (
            IEnumerable<ITemplateToken<TState>> existingTokens,
            string value,
            string variable,
            Func<TValue, string> getValueDelegate,
            Func<string> defaultValueFunc = null
        ) where TValue : ITemplateToken<TState>
            => value.IndexOf(variable) == -1
                ? value
                : value.Replace
                    (
                        variable,
                        existingTokens
                            .GetTemplateTokensFromSections<TState, TValue>()
                            .Select(getValueDelegate)
                            .LastOrDefaultWhenEmpty(() => defaultValueFunc?.Invoke() ?? string.Empty) //design decision: when multiple values are found, use the last one
                    );

        private static string PathSafeCombine(string path1, string path2)
        {
            try
            {
                return Path.Combine(path1, path2);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}
