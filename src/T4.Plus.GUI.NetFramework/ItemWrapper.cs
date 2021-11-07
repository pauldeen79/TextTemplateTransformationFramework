using System;

namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    public class ItemWrapper<T>
    {
        public T Item { get; }
        public Func<T, string> DisplayMemberDelegate { get; }

        public ItemWrapper(T item, Func<T, string> displayMemberDelegate)
        {
            Item = item;
            DisplayMemberDelegate = displayMemberDelegate;
        }

        public override string ToString()
        {
            return DisplayMemberDelegate(Item);
        }
    }
}
