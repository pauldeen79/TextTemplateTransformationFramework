using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    public sealed class ParametersViewModel : ICustomTypeDescriptor
    {
        private readonly List<TemplateParameter> _parameters;

        public ParametersViewModel(IEnumerable<TemplateParameter> parameters)
        {
            _parameters = parameters.ToList();
        }

        #region ICustomTypeDescriptor
        AttributeCollection ICustomTypeDescriptor.GetAttributes() => TypeDescriptor.GetAttributes(this, true);

        string ICustomTypeDescriptor.GetClassName() => TypeDescriptor.GetClassName(this, true);

        string ICustomTypeDescriptor.GetComponentName() => TypeDescriptor.GetComponentName(this, true);

        TypeConverter ICustomTypeDescriptor.GetConverter() => TypeDescriptor.GetConverter(this, true);

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(attributes, true);

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => ((ICustomTypeDescriptor)this).GetEvents(null);

        /// <summary>
        /// Returns the properties for this instance of a component using the attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="Attribute" /> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="PropertyDescriptorCollection" /> that represents the filtered properties for this component instance.
        /// </returns>
        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            var props = new List<PropertyDescriptor>();

            foreach (var param in _parameters)
            {
                AddPropertyToCustomTypeDescriptor(props, param);
            }

            if (attributes?.Length > 0)
            {
                var arrayList = FilterMembers(props, attributes);
                if (arrayList != null)
                {
                    return new PropertyDescriptorCollection(arrayList.ToArray(), true);
                }
            }
            return new PropertyDescriptorCollection(props.ToArray());
        }

        private void AddPropertyToCustomTypeDescriptor(List<PropertyDescriptor> props, TemplateParameter param)
        {
            if (props.Any(p => p.Name == param.Name))
                return; //already registered (is already a strongly-typed property)

            var attributesList = new List<Attribute>();

            if (!string.IsNullOrEmpty(param.DisplayName))
                attributesList.Add(new DisplayNameAttribute(param.DisplayName));

            if (!string.IsNullOrEmpty(param.Description))
                attributesList.Add(new DescriptionAttribute(param.Description));

            if (!param.Browsable)
                attributesList.Add(new BrowsableAttribute(false));

            if (param.ReadOnly)
                attributesList.Add(new ReadOnlyAttribute(true));

            if (!string.IsNullOrEmpty(param.EditorAttributeEditorTypeName))
            {
                attributesList.Add(new EditorAttribute(param.EditorAttributeEditorTypeName, param.EditorAttributeEditorBaseTypeName));
            }

            if (!string.IsNullOrEmpty(param.TypeConverterTypeName))
            {
                attributesList.Add(new TypeConverterAttribute(param.TypeConverterTypeName));
            }

            if (param.PossibleValues?.Length > 0)
            {
                attributesList.Add(new CategoryAttribute(string.Join("|", param.PossibleValues)));
            }

            props.Add(new ParameterPropertyDescriptor(param.Name, attributesList.ToArray(), param, this.GetType())); //always add all atributes! the filter gets applied when GetProperties is called.
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => ((ICustomTypeDescriptor)this).GetProperties(null);

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => this;
        #endregion

        public IEnumerable<TemplateParameter> GetParameters()
        {
            var lst = new List<TemplateParameter>();
            foreach(var item in _parameters)
            {
                lst.Add(new TemplateParameter
                {
                    Name = item.Name,
                    OmitValueAssignment = item.OmitValueAssignment,
                    Type = item.Type,
                    Value = item.Value,
                    Description = item.Description,
                    DisplayName = item.DisplayName,
                    Browsable = item.Browsable,
                    ReadOnly = item.ReadOnly,
                    PossibleValues = item.PossibleValues
                });
            }
            return lst;
        }

        private static IList<PropertyDescriptor> FilterMembers(IList<PropertyDescriptor> members, Attribute[] attributes)
        {
            IList<PropertyDescriptor> arrayList = null;
            int count = members.Count;
            for (int i = 0; i < count; i++)
            {
                bool flag = false;
                for (int j = 0; j < attributes.Length; j++)
                {
                    if (ShouldHideMember(members[i], attributes[j]))
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    if (arrayList == null)
                    {
                        arrayList = new List<PropertyDescriptor>(count);
                        for (int k = 0; k < i; k++)
                        {
                            arrayList.Add(members[k]);
                        }
                    }
                }
                else
                {
                    arrayList?.Add(members[i]);
                }
            }
            return arrayList;
        }

        private static bool ShouldHideMember(MemberDescriptor member, Attribute attribute)
        {
            if (member == null || attribute == null)
            {
                return true;
            }
            Attribute attribute2 = member.Attributes[attribute.GetType()];
            if (attribute2 == null)
            {
                return !attribute.IsDefaultAttribute();
            }
            return !attribute.Match(attribute2);
        }
    }
}
