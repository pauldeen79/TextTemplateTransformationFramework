namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IProcessInitializer<in TContext>
        where TContext : class
    {
        void Initialize(TContext context);
    }
}
