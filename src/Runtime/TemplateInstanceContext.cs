using System;

namespace TextTemplateTransformationFramework.Runtime
{
    public class TemplateInstanceContext
    {
        public object Template { get; private set; }
        public object Model { get; set; }
        public object ViewModel { get; set; }
        public string ResolveTemplateName { get; private set; }
        public TemplateInstanceContext ParentContext { get; private set; }

        public TemplateInstanceContext RootContext
        {
            get
            {
                var p = this;
                while (p?.ParentContext != null)
                {
                    p = p.ParentContext;
                }
                return p;
            }
        }

        public T GetModelFromContextByType<T>(Func<TemplateInstanceContext, bool> predicate = null)
        {
            var p = this;
            while (p != null)
            {
                if (p.Model is T t && (predicate == null || predicate(p)))
                {
                    return t;
                }
                p = p.ParentContext;
            }
            return default;
        }

        public T GetViewModelFromContextByType<T>(Func<TemplateInstanceContext, bool> predicate = null)
        {
            var p = this;
            while (p != null)
            {
                if (p.ViewModel is T t && (predicate == null || predicate(p)))
                {
                    return t;
                }
                p = p.ParentContext;
            }
            return default;
        }

        public T GetContextByType<T>(Func<TemplateInstanceContext, bool> predicate = null)
        {
            var p = this;
            while (p != null)
            {
                if (p.Template is T t && (predicate == null || predicate(p)))
                {
                    return t;
                }
                p = p.ParentContext;
            }
            return default;
        }

        public bool IsRootContext
        {
            get
            {
                return ParentContext == null;
            }
        }

        public TemplateInstanceContext CreateChildContext(object template, object model, object viewModel = null, int? iterationNumber = null, int? iterationCount = null, string resolveTemplateName = null)
        {
            return new TemplateInstanceContext
            {
                Template = template,
                Model = model,
                ViewModel = viewModel,
                ParentContext = this,
                IterationNumber = iterationNumber,
                IterationCount = iterationCount,
                ResolveTemplateName = resolveTemplateName
            };
        }

        public static TemplateInstanceContext CreateRootContext(object template)
        {
            return new TemplateInstanceContext
            {
                Template = template
            };
        }

        public int? IterationNumber { get; set; }
        public int? IterationCount { get; set; }

        public bool IsFirstIteration
        {
            get
            {
                return IterationNumber.HasValue
                    && IterationCount.HasValue
                    && IterationNumber.Value == 0;
            }
        }

        public bool IsLastIteration
        {
            get
            {
                return IterationNumber.HasValue
                    && IterationCount.HasValue
                    && IterationNumber.Value + 1 == IterationCount.Value;
            }
        }
    }
}