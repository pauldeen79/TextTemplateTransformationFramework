using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.ProcessInitializers
{
    public class EmptyProcessInitializer<TContext> : IProcessInitializer<TContext>
        where TContext : class
    {
        public void Initialize(TContext context)
        {
            // Method left empty intentionally.
        }
    }
}
