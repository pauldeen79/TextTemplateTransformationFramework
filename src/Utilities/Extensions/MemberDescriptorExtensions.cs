using System;
using System.ComponentModel;
using System.Linq;

namespace Utilities.Extensions
{
    public static class MemberDescriptorExtensions
    {
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="T">Type of  attribute to get.</typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberDescriptor instance)
            where T : Attribute
        {
            return instance.Attributes.OfType<T>().FirstOrDefault();
        }
    }
}
