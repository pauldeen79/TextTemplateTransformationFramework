using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class TestModel
    {
        [Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public bool? OptionalBoolean { get; set; }

        public int Number { get; set; }

        public int? OptionalNumber { get; set; }

        public string[] List { get; set; }

        [TypeConverter(typeof(LanguageEnumConverter))]
        public Language Language { get; set; }
    }

    public enum Language { VbNet = 1, CSharp = 0 }

    [ExcludeFromCodeCoverage]
    public class LanguageEnumConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string str)
            {
                switch (str.ToLower(CultureInfo.InvariantCulture))
                {
                    case "vb":
                    case "vbnet":
                        return Language.VbNet;
                    case "cs":
                    case "csharp":
                    case "c#":
                        return Language.CSharp;
                    default:
                        Language val;
                        if (Enum.TryParse(str, out val))
                        {
                            return val;
                        }
                        break;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Language)
            {
                return ((Language)value) == Language.VbNet
                    ? "vb"
                    : "cs";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
