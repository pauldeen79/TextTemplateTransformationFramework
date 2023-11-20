using System;
using System.ComponentModel;
using System.Linq;

namespace TextTemplateTransformationFramework.Runtime.ComponentModel
{
    public class FormatStringConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new StandardValuesCollection(context.PropertyDescriptor.Attributes.OfType<CategoryAttribute>().First().Category.Split('|'));
        }
    }
}
