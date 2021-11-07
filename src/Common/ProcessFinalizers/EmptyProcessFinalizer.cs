using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.ProcessFinalizers
{
    public class EmptyProcessFinalizer<TContext> : IProcessFinalizer<TContext>
        where TContext : class
    {
        public void Finalize(TContext context)
        {
            // Method left empty intentionally.
        }
    }
}
