using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using CrossCutting.Common.Testing;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase
    {
        protected IFixture Fixture { get; } = new Fixture().Customize(new AutoNSubstituteCustomization());

        protected void ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(Type type)
            => type.ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(x => Fixture.Create(x, new SpecimenContext(Fixture)));
    }
}
