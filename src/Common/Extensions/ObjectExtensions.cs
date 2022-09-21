using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static TemplateParameter ToParameter(this object template, PropertyDescriptor property)
            => ScopedMember.Evaluate
            (
                property.GetAttribute<EditorAttribute>(),
                editorAttribute =>
                new TemplateParameter
                {
                    Name = property.Name,
                    Type = property.PropertyType,
                    Value = property.GetAttribute<DefaultValueAttribute>().Either((v) => v.Value, () => property.GetValue(template)),
                    DisplayName = property.GetAttribute<DisplayNameAttribute>()?.DisplayName,
                    Description = property.GetAttribute<DescriptionAttribute>()?.Description,
                    Browsable = property.GetAttribute<BrowsableAttribute>().Either((v) => v.Browsable, () => true),
                    ReadOnly = property.GetAttribute<ReadOnlyAttribute>().Either((v) => v.IsReadOnly, () => false),
                    EditorAttributeEditorTypeName = editorAttribute?.EditorTypeName,
                    EditorAttributeEditorBaseTypeName = editorAttribute?.EditorBaseTypeName,
                    TypeConverterTypeName = property.GetAttribute<TypeConverterAttribute>()?.ConverterTypeName,
                    PossibleValues = property.GetAttribute<CategoryAttribute>().Either(ca => ca.Category.Split('|'), Array.Empty<string>)
                }
            );

        public static object TryGetModel(this object instance, Type modelType)
        {
            if (instance is IModelTypeCreator modelTypeCreator)
            {
                var t = modelTypeCreator.CreateType(modelType);
                return t.IsGenericTypeDefinition
                    ? Activator.CreateInstance(t.MakeGenericType(modelType))
                    : Activator.CreateInstance(t);
            }

            return instance;
        }

        public static void TrySetFileNameProvider(this object instance, IFileNameProvider fileNameProvider)
        {
            if (instance is IFileNameProviderContainer fileNameProviderContainer)
            {
                fileNameProviderContainer.FileNameProvider = fileNameProvider;
            }
        }

        public static void TrySetFileContentsProvider(this object instance, IFileContentsProvider fileContentsProvider)
        {
            if (instance is IFileContentsProviderContainer fileContentsProviderContainer)
            {
                fileContentsProviderContainer.FileContentsProvider = fileContentsProvider;
            }
        }

        public static void TrySetTemplateCodeCompiler<TState>(this object instance, ITemplateCodeCompiler<TState> templateCodeCompiler)
            where TState : class
        {
            if (instance is ITemplateCodeCompilerContainer<TState> templateCodeCompilerContainer)
            {
                templateCodeCompilerContainer.TemplateCodeCompiler = templateCodeCompiler;
            }
        }

        public static object ConvertValue(this object instance, Type type)
        {
            if (instance == null)
            {
                return null;
            }

            if (type.IsInstanceOfType(instance))
            {
                return instance;
            }

            if (instance is int && type.IsEnum)
            {
                return Enum.ToObject(type, instance);
            }

            return Convert.ChangeType(instance, type);
        }
    }
}
