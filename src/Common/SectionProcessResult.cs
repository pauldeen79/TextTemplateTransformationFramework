using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Data;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common
{
    public static class SectionProcessResult
    {
        public static SectionProcessResult<TState> Create<TState>
        (
            ITemplateToken<TState> token,
            int? switchToMode = null,
            bool passThrough = false,
            bool tokensAreForRootTemplateSection = false
        ) where TState : class
            => new SectionProcessResult<TState>
            (
                true,
                switchToMode,
                passThrough,
                tokensAreForRootTemplateSection,
                token.AsEnumerable()
            );

        public static SectionProcessResult<TState> Create<TState>
        (
            IEnumerable<ITemplateToken<TState>> tokens,
            int? switchToMode = null,
            bool passThrough = false,
            bool tokensAreForRootTemplateSection = false,
            SectionProcessResult<TState> existingResult = null,
            Type customProcessorType = null
        ) where TState : class
            => new SectionProcessResult<TState>
            (
                true,
                switchToMode,
                passThrough,
                tokensAreForRootTemplateSection,
                existingResult is not null
                    ? existingResult.Tokens.Concat(tokens)
                    : tokens,
                customProcessorType
            );

        public static SectionProcessResult<TState> Create<TState>
        (
            SectionContext<TState> context,
            int switchToMode,
            Func<string, ITemplateToken<TState>> createTokenDelegate
        ) where TState : class
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (createTokenDelegate is null)
            {
                throw new ArgumentNullException(nameof(createTokenDelegate));
            }

            var tokens = createTokenDelegate(context.Section.Substring(1).Trim());
            return context.CurrentMode != switchToMode
                ? Create(tokens, switchToMode)
                : Create(tokens);
        }

        public static SectionProcessResult<TState> Create<TState>
        (
            SectionContext<TState> context,
            ICollection<ValidationResult> validationResults,
            int? switchToMode = null,
            bool passThrough = false,
            bool tokensAreForRootTemplateSection = false
        ) where TState : class
            => new SectionProcessResult<TState>
            (
                true,
                switchToMode,
                passThrough,
                tokensAreForRootTemplateSection,
                validationResults.Select(e => new InitializeErrorToken<TState>(context, e.ErrorMessage))
            );

        public static SectionProcessResult<TState> Create<TState>(SectionProcessResultData<TState> data)
            where TState : class
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.Validate();

            //First validate the model...
            if (!data.Model.TryValidate(out ICollection<ValidationResult> validationResults))
            {
                return Create(data.Context, validationResults, data.SwitchToMode, data.PassThrough, data.TokensAreForRootTemplateSection);
            }

            //Then check whether the mapper should process...
            if (!data.IsValidDelegate(data.Context, data.Model))
            {
                return data.ExistingResult ?? (data.PassThrough
                    ? SectionProcessResult<TState>.EmptyPassThrough
                    : SectionProcessResult<TState>.Empty);
            }

            var totalResults = new List<ITemplateToken<TState>>();

            //Generate warnings when properties have a non-default value and ObsoleteAttribute
            var properties = data.Model
                .GetType()
                .GetProperties()
                .Select(p => new
                {
                    Property = p,
                    ObsoleteAttribute = p.GetCustomAttribute<ObsoleteAttribute>()
                })
                .Where(p => p.ObsoleteAttribute is not null)
                .Select(p => new
                {
                    p.Property,
                    ObsoleteAttributeMessage = p.ObsoleteAttribute.Message ?? $"Property {p.Property.Name} on {data.DirectiveName ?? data.Model.GetType().Name} is obsolete",
                    DefaultValueAttribute = p.Property.GetCustomAttribute<DefaultValueAttribute>()
                })
                .Where
                (
                    p => PropertyHasNonDefaultValue(p.Property, p.DefaultValueAttribute, data.Model)
                )
                .ToArray();
            totalResults.AddRange(properties.Where(property => data.ExistingResult?.Tokens.Any(t => t.SectionContext.LineNumber == data.Context.LineNumber && t is InitializeWarningToken<TState> token && token.Message == property.ObsoleteAttributeMessage) != true).Select(property => new InitializeWarningToken<TState>(data.Context, property.ObsoleteAttributeMessage)));
            AddObsoleteWarnings(data.Context, data.Model, data.ExistingResult, data.DirectiveName, totalResults);

            //If everything is alright, then map the model to tokens
            var mapResult = data.MapDelegate();
            if (mapResult is ITemplateToken<TState> singleResult)
            {
                totalResults.Add(singleResult);
            }
            else if (mapResult is IEnumerable<ITemplateToken<TState>> multipleResult)
            {
                totalResults.AddRange(multipleResult);
            }
            else
            {
                throw new InvalidOperationException("Return value of mapper is not of type ITemplateToken or IEnumerable<ITemplateToken>");
            }

            return Create
            (
                totalResults,
                data.SwitchToMode,
                data.PassThrough,
                data.TokensAreForRootTemplateSection,
                data.ExistingResult,
                data.ExistingResult?.CustomProcessorType
            );
        }

        private static void AddObsoleteWarnings<TState>(SectionContext<TState> context, object model, SectionProcessResult<TState> existingResult, string directiveName, List<ITemplateToken<TState>> totalResults) where TState : class
        {
            var obsoleteAttribute = model.GetType().GetCustomAttribute<ObsoleteAttribute>();
            if (obsoleteAttribute is not null)
            {
                var obsoleteMessage = obsoleteAttribute.Message ?? $"{directiveName ?? model.GetType().Name} is obsolete";
                if (existingResult?.Tokens.Any(t => t.SectionContext.LineNumber == context.LineNumber && t is InitializeWarningToken<TState> token && token.Message == obsoleteMessage) != true)
                {
                    totalResults.Add(new InitializeWarningToken<TState>(context, obsoleteMessage));
                }
            }
        }

        private static bool PropertyHasNonDefaultValue(PropertyInfo property, DefaultValueAttribute defaultValueAttribute, object model)
        {
            var actualValue = property.GetValue(model);
            var defaultValue = defaultValueAttribute is not null
                ? defaultValueAttribute.Value
                : property.PropertyType.GetDefaultValue();

            if (actualValue is null && defaultValue is null)
            {
                return false;
            }

            return actualValue is null
                || defaultValue is null
                || !actualValue.Equals(defaultValue);
        }
    }

    public sealed class SectionProcessResult<TState>
        where TState: class
    {
        public bool Understood { get; }

        public int? SwitchToMode { get; }

        public bool PassThrough { get; }

        public bool TokensAreForRootTemplateSection { get; }

        public IEnumerable<ITemplateToken<TState>> Tokens { get; }

        public Type CustomProcessorType { get; }

        internal SectionProcessResult
        (
            bool understood,
            int? switchToMode,
            bool passThrough,
            bool tokensAreForRootTemplateSection,
            IEnumerable<ITemplateToken<TState>> tokens,
            Type customProcessorType = null
        )
        {
            Understood = understood;
            SwitchToMode = switchToMode;
            PassThrough = passThrough;
            Tokens = tokens?.ToArray() ?? Array.Empty<ITemplateToken<TState>>();
            TokensAreForRootTemplateSection = tokensAreForRootTemplateSection;
            CustomProcessorType = customProcessorType;
        }

        public static readonly SectionProcessResult<TState> NotUnderstood = new SectionProcessResult<TState>(false, null, false, false, Array.Empty<ITemplateToken<TState>>());
        public static readonly SectionProcessResult<TState> Empty = new SectionProcessResult<TState>(true, null, false, false, Array.Empty<ITemplateToken<TState>>());
        public static readonly SectionProcessResult<TState> EmptyPassThrough = new SectionProcessResult<TState>(true, null, true, false, Array.Empty<ITemplateToken<TState>>());
    }
}
