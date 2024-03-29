﻿using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class AssemblyTests : TestBase
    {
        [Fact]
        public void AssemblyDoesNotContainInterfacesWithTooManyMethods()
        {
            // Act
            var types = typeof(TextTemplate).Assembly.GetExportedTypes()
                .Where(t => t.IsInterface)
                .Select(t => new { TypeName = t.FullName.FixTypeName(), MethodCount = t.GetMethods().Count(m => !m.IsSpecialName) })
                .Where(x => x.MethodCount > 5)
                .OrderByDescending(x => x.MethodCount)
                .ToArray();

            // Assert
            types.Should().BeEmpty();
        }

        [Fact]
        public void AssemblyDoesNotContainInterfaceMethodsWithTooManyArguments()
        {
            // Act
            var methods = typeof(TextTemplate).Assembly.GetExportedTypes()
                .Where(t => t.IsInterface)
                .SelectMany(t => t.GetMethods().Select(m => new { TypeName = t.FullName.FixTypeName(), MethodName = m.Name, MethodArgumentCount = m.GetParameters().Length }))
                .Where(x => x.MethodArgumentCount > 3)
                .OrderByDescending(x => x.MethodArgumentCount)
                .ToArray();

            // Assert
            methods.Should().BeEmpty();
        }

        [Fact]
        public void AssemblyDoesNotContainClassesWithTooManyConstructorArguments()
        {
            // Act
            var ctors = typeof(TextTemplate).Assembly.GetExportedTypes()
                .SelectMany(t => t.GetConstructors().Select(c => new { TypeName = t.FullName.FixTypeName(), ConstructorArgumentCount = c.GetParameters().Count(p => p.ParameterType.IsInterface) }))
                .Where(x => x.ConstructorArgumentCount > 7)
                .OrderByDescending(x => x.ConstructorArgumentCount)
                .ToArray();

            // Assert
            ctors.Should().BeEmpty();
        }

        [Fact]
        public void AssemblyDoesNotContainClassesWhichAllowNullArgumentsInCtor()
        {
            // Arrange
            var types = typeof(TextTemplate).Assembly.GetExportedTypes()
                .Where(t => t.Name != nameof(Section) && !t.IsInterface && !t.IsAbstract && t.GetGenericArguments().Length == 0)
                .Select(t => new
                {
                    Type = t,
                    Constructors = t.GetConstructors().Where
                    (
                        x => x.GetParameters().Length > 0
                        && !Array.Exists(x.GetParameters(),
                            y => y.ParameterType != typeof(string)
                            && typeof(IEnumerable).IsAssignableFrom(y.ParameterType)
                        )
                        && !Array.Exists(x.GetParameters(), y => y.ParameterType.IsGenericType)
                    ).ToArray()
                })
                .Where(x => x.Constructors.Length == 1)
                .Select(x => x.Type)
                .ToList();

            types.ForEach(x => ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(x, pi => pi.Name != nameof(CompilerError.FileName).ToPascalCase()));
        }
    }
}
