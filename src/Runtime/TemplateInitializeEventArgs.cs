using System;

namespace TextTemplateTransformationFramework.Runtime
{
    public class TemplateInitializeEventArgs : EventArgs
    {
        public TemplateWrapper TemplateWrapper { get; }

        public TemplateInitializeEventArgs(TemplateWrapper templateWrapper)
        {
            TemplateWrapper = templateWrapper;
        }
    }
}
