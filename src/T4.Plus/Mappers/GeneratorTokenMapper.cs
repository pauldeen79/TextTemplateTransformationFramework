﻿using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(GeneratorDirectiveModel))]
    [DirectivePrefix("generator")]
    public sealed class GeneratorTokenMapper<TState> : ISingleTokenMapper<TState, GeneratorDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, GeneratorDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new GeneratorToken<TState>(context, model.Name, model.Version);
        }
    }
}
