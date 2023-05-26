#if Windows
using System;
using System.IO;
using System.Runtime.Loader;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class AssemblyServiceTests
    {
        [Fact]
        public void GetCustomPaths_Throws_On_Null_AssemblyName()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act & Assert
            sut.Invoking(x => x.GetCustomPaths(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetCustomPaths_Returns_Empty_Array_When_AssemblyName_Is_Not_A_Filename()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act
            var actual = sut.GetCustomPaths(GetType().Assembly.FullName);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void GetCustomPaths_Returns_Filled_Array_With_Correct_DirectoryName_When_AssemblyName_Is_A_Filename()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act
            var actual = sut.GetCustomPaths("TextTemplateTransformationFramework.Common.Tests.dll");

            // Assert
            actual.Should().BeEquivalentTo(Directory.GetCurrentDirectory());
        }

        [Fact]
        public void LoadAssembly_Throws_On_Null_AssemblyName()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act & Assert
            sut.Invoking(x => x.LoadAssembly(null, AssemblyLoadContext.Default)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LoadAssembly_Throws_On_Null_AssemblyLoadContext()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act & Assert
            sut.Invoking(x => x.LoadAssembly("TextTemplateTransformationFramework.Common.Tests.dll", null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LoadAssembly_Loads_Correct_Assembly_From_AssemblyName()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act
            var assembly = sut.LoadAssembly(GetType().Assembly.FullName, AssemblyLoadContext.Default);

            // Assert
            assembly.Should().BeSameAs(GetType().Assembly);
        }

        [Fact]
        public void LoadAssembly_Loads_Correct_Assembly_From_Fully_Qualified_FileName()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act
            var assembly = sut.LoadAssembly(GetType().Assembly.Location, AssemblyLoadContext.Default);

            // Assert
            assembly.Should().BeSameAs(GetType().Assembly);
        }

        [Fact]
        public void LoadAssembly_Loads_Correct_Assembly_From_Relative_FileName()
        {
            // Arrange
            var sut = new AssemblyService();

            // Act
            var assembly = sut.LoadAssembly("TextTemplateTransformationFramework.Common.Tests.dll", AssemblyLoadContext.Default);

            // Assert
            assembly.Should().BeSameAs(GetType().Assembly);
        }
    }
}
#endif
