using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.LanguageServices
{
    public class DirectiveSerializer<TState, TToken>
        where TState : class
    {
        private readonly Func<string, string> _singleValueProvider;
        private readonly Func<string, IEnumerable<string>> _multiValueProvider;
        private readonly IFileNameProvider _fileNameProvider;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;
        private readonly SectionContext<TState> _context;

        public DirectiveSerializer(Func<string, string> singleValueProvider,
                                   Func<string, IEnumerable<string>> multiValueProvider,
                                   IFileNameProvider fileNameProvider,
                                   IFileContentsProvider fileContentsProvider,
                                   ITemplateCodeCompiler<TState> templateCodeCompiler)
        {
            _singleValueProvider = singleValueProvider;
            _multiValueProvider = multiValueProvider;
            _fileNameProvider = fileNameProvider ?? throw new ArgumentNullException(nameof(fileNameProvider));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
        }

        public DirectiveSerializer(SectionContext<TState> context,
                                   IFileNameProvider fileNameProvider,
                                   IFileContentsProvider fileContentsProvider,
                                   ITemplateCodeCompiler<TState> templateCodeCompiler)
            : this(name => context.TokenParserCallback.GetSectionArgument(context, name),
                   name => context.TokenParserCallback.GetSectionArguments(context, name),
                   fileNameProvider,
                   fileContentsProvider,
                   templateCodeCompiler)
        {
            _context = context;
        }

        public DirectiveSerializer(IArgumentParser argumentParser,
                                   string section,
                                   IFileNameProvider fileNameProvider,
                                   IFileContentsProvider fileContentsProvider,
                                   ITemplateCodeCompiler<TState> templateCodeCompiler)
            : this(name => argumentParser.Parse(section, name).FirstOrDefault(),
                   name => argumentParser.Parse(section, name),
                   fileNameProvider,
                   fileContentsProvider,
                   templateCodeCompiler)
        {
        }

        /// <summary>
        /// Deserializes the specified directive into an object instance.
        /// </summary>
        /// <returns></returns>
        public TToken Deserialize()
        {
            var type = typeof(TToken).GetModelType(typeof(TState));
            var instance = Activator.CreateInstance(type);
            var sectionContextContainer = instance as ISectionContextContainer<TState>;
            if (sectionContextContainer != null)
            {
                sectionContextContainer.SectionContext = _context;
            }
            instance.TrySetFileNameProvider(_fileNameProvider);
            instance.TrySetFileContentsProvider(_fileContentsProvider);
            instance.TrySetTemplateCodeCompiler(_templateCodeCompiler);

            //First, set all attributes that are found in the directive
            foreach (var attribute in GetDirectiveAttributes(type, sectionContextContainer != null))
            {
                if (TypeIsEnumerable(attribute.PropertyInfo.PropertyType))
                {
                    var value = _multiValueProvider(attribute.Name);
                    SetMultiPropertyValue(attribute.PropertyInfo, instance, value);
                }
                else
                {
                    var value = _singleValueProvider(attribute.Name);
                    SetSinglePropertyValue(attribute.PropertyInfo, instance, value);
                }
            }

            return (TToken)instance;
        }

        /// <summary>
        /// Serializes the specified instance into a directive.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public string Serialize(TToken instance)
        {
            var type = typeof(TToken);
            var stringBuilder = new StringBuilder();
            foreach (var attribute in GetDirectiveAttributes(type, instance is ISectionContextContainer<TState>))
            {
                var val = attribute.PropertyInfo.GetValue(instance, null);
                var typeConverterAttribute = attribute.PropertyInfo.GetCustomAttribute<TypeConverterAttribute>(true);
                var defaultValueAttribute = attribute.PropertyInfo.GetCustomAttribute<DefaultValueAttribute>(true);
                var isDefaultValue = GetIsDefaultValue(attribute, val, defaultValueAttribute);

                if (isDefaultValue)
                {
                    //default value, do nothing
                }
                else if (typeConverterAttribute != null)
                {
                    var typeConverter = (TypeConverter)Activator.CreateInstance(Type.GetType(typeConverterAttribute.ConverterTypeName));
                    var convertedValue = typeConverter.ConvertToString(val);
                    AppendToStringBuilder(stringBuilder, attribute.Name, convertedValue);
                }
                else if (TypeIsEnumerable(val.GetType()))
                {
                    foreach (var item in ((IEnumerable)val))
                    {
                        AppendToStringBuilder(stringBuilder, attribute.Name, item);
                    }
                }
                else
                {
                    //all other types (including int) are handled automatically by string.Format, which calls ToString() on the object.
                    AppendToStringBuilder(stringBuilder, attribute.Name, val is string ? val : val.CsharpFormat());
                }
            }

            return stringBuilder.ToString();
        }

        private static bool GetIsDefaultValue(DirectiveAttribute attribute, object val, DefaultValueAttribute defaultValueAttribute)
        {
            if (defaultValueAttribute != null)
            {
                return val.Equals(defaultValueAttribute.Value);
            }
            
            if (attribute.PropertyInfo.PropertyType == typeof(bool) && val != null)
            {
                return val.Equals(default(bool));
            }
            
            return attribute.PropertyInfo.PropertyType == typeof(int) && val != null
                ? val.Equals(default(int))
                : val == null;
        }

        /// <summary>
        /// Gets the directive attributes.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="skipSectionContext">If set to true, then skip SectionContext</param>
        /// <returns></returns>
        private static IEnumerable<DirectiveAttribute> GetDirectiveAttributes(Type type, bool skipSectionContext)
            => type
                .GetProperties()
                .Where
                (
                    p => p.CanRead
                        && p.CanWrite
                        && p.GetGetMethod() != null
                        && p.GetSetMethod() != null
                        && !(skipSectionContext && p.Name == "SectionContext")
                )
                .Select(propertyInfo => new DirectiveAttribute(propertyInfo, GetAttributeName(propertyInfo)));

        /// <summary>
        /// Determines whether the specified type is enumerable. (and not a string)
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// true when enumerable (and not string), otherwise false.
        /// </returns>
        private static bool TypeIsEnumerable(Type type)
            => typeof(IEnumerable).IsAssignableFrom(type)
                && type != typeof(string);

        /// <summary>
        /// Gets the name of the attribute.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>
        /// Attribute name, or null when it cannot be determined.
        /// </returns>
        private static string GetAttributeName(PropertyInfo propertyInfo)
        {
            var dataMemberAttribute = propertyInfo
                .GetCustomAttributes(typeof(DataMemberAttribute), true)
                .Cast<DataMemberAttribute>()
                .FirstOrDefault();

            //Note that when there is no DataMemberAttribute, we assume the attribute name is equal to the property name. (note that the get value delegates are responsible for case sensitivity)
            return dataMemberAttribute == null
                ? propertyInfo.Name
                : dataMemberAttribute.Name;
        }

        /// <summary>
        /// Sets the multi property value.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        private static void SetMultiPropertyValue(PropertyInfo propertyInfo, object instance, IEnumerable<string> value)
        {
            if (propertyInfo.PropertyType == typeof(string[]))
            {
                propertyInfo.SetValue(instance, value.ToArray(), null);
            }
            else if (propertyInfo.PropertyType == typeof(bool[]))
            {
                propertyInfo.SetValue(instance, value?.Select(s => s.IsTrue()).ToArray(), null);
            }
            else
            {
                propertyInfo.SetValue(instance, Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
            }
        }

        /// <summary>
        /// Sets the single property value.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="NotSupportedException"></exception>
        private static void SetSinglePropertyValue(PropertyInfo propertyInfo, object instance, string value)
        {
            if (value == null)
            {
                //empty attribute values don't need to be set, unless there is a default value attribute
                var defaultValueAttribute = propertyInfo.GetCustomAttribute<DefaultValueAttribute>(true);
                if (defaultValueAttribute != null)
                {
                    propertyInfo.SetValue(instance, defaultValueAttribute.Value, null);
                }
                return;
            }

            var typeConverterAttribute = propertyInfo
                .GetCustomAttributes(typeof(TypeConverterAttribute), true)
                .Cast<TypeConverterAttribute>()
                .FirstOrDefault();

            if (typeConverterAttribute != null)
            {
                var typeConverter = (TypeConverter)Activator.CreateInstance(Type.GetType(typeConverterAttribute.ConverterTypeName));
                var convertedValue = typeConverter.ConvertFromString(value);
                propertyInfo.SetValue(instance, convertedValue, null);
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(instance, value, null);
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                propertyInfo.SetValue(instance, Convert.ToBoolean(value), null);
            }
            else if (propertyInfo.PropertyType == typeof(bool?))
            {
                propertyInfo.SetValue(instance, string.IsNullOrEmpty(value) ? (bool?)null : Convert.ToBoolean(value), null);
            }
            else
            {
                propertyInfo.SetValue(instance, Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture), null);
            }
        }

        /// <summary>
        /// Appends the specified value to the specified string builder.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        private static void AppendToStringBuilder(StringBuilder stringBuilder, string name, object value)
        {
            if (stringBuilder.Length > 0)
            {
                stringBuilder.Append(" ");
            }

            var formattedValue = value.ToStringWithNullCheck().Replace("\"", "\\\"");

            stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0}=\"{1}\"", name, formattedValue);
        }

        private sealed class DirectiveAttribute
        {
            public DirectiveAttribute(PropertyInfo propertyInfo, string name)
            {
                PropertyInfo = propertyInfo;
                Name = name;
            }

            public string Name { get; }
            public PropertyInfo PropertyInfo { get; }
        }
    }
}
