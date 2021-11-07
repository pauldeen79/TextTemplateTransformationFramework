using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    public class DataContractSerializerTests
    {
        [Fact]
        public void CanSerializeDataContractWithICollection()
        {
            // Arrange
            var sut = new DataContractSerializer(typeof(MyRequest));
            var req = new MyRequest
            {
                Name = "Name",
                Collection = new List<MyItem>(new[] { new MyItem { Name = "SubName" } })
            };

            // Act
            using (var stream = new MemoryStream())
            {
                sut.WriteObject(stream, req);
                var result = Encoding.UTF8.GetString(stream.ToArray());

                // Assert
                result.Should().Be(@"<MyRequest xmlns=""http://schemas.datacontract.org/2004/07/Utilities.Tests"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Collection><MyItem><Name>SubName</Name></MyItem></Collection><Name>Name</Name></MyRequest>");
            }
        }

        [Fact]
        public void CanDeserializeDataContractWithICollection()
        {
            // Arrange
            var sut = new DataContractSerializer(typeof(MyRequest));
            const string Xml = @"<MyRequest xmlns=""http://schemas.datacontract.org/2004/07/Utilities.Tests"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""><Collection><MyItem><Name>SubName</Name></MyItem></Collection><Name>Name</Name></MyRequest>";

            // Act
            using (var stream = new MemoryStream())
            {
                stream.Write(Encoding.UTF8.GetBytes(Xml));
                stream.Flush();
                stream.Position = 0;
                var actual = (MyRequest)sut.ReadObject(stream);

                // Assert
                actual.Should().NotBeNull();
            }
        }
    }

    [DataContract]
    [ExcludeFromCodeCoverage]
    public class MyRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ICollection<MyItem> Collection { get; set; }
    }

    [DataContract]
    [ExcludeFromCodeCoverage]
    public class MyItem
    {
        [DataMember]
        public string Name { get; set; }
    }
}
