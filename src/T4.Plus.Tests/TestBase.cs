using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using CrossCutting.Common.Testing;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase
    {
        protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

        protected void ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(Type type, Func<ParameterInfo, bool> parameterPredicate = null)
            => type.ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(x => Fixture.Create(x, new SpecimenContext(Fixture)), parameterPredicate);
    }
}
