using System;
using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public class TemplateWrapper : T4PlusGeneratedTemplateBase
    {
        public object Model { get; set; }
        public object ViewModel { get; set; }
        public Tuple<string, Func<object>, Type> Registration { get; }
        public event EventHandler<TemplateInitializeEventArgs> OnInitialize;
        public event EventHandler<TemplateRenderEventArgs> OnRender;

        private readonly object _rootTemplate;

        private object _currentInstance;
        private Action _additionalActionDelegate;

        public TemplateWrapper(string templateName, object templateInstance, Type modelType, object rootTemplate = null, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null)
            : this(templateName, () => templateInstance, modelType, rootTemplate, onInitializeEventHandler, onRenderEvenHandler)
        {
        }

        public TemplateWrapper(string templateName, Func<object> templateCreationDelegate, Type modelType, object rootTemplate = null, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null)
            : this(new Tuple<string, Func<object>, Type>(templateName, templateCreationDelegate, modelType), rootTemplate, onInitializeEventHandler, onRenderEvenHandler)
        {
        }

        public TemplateWrapper(Tuple<string, Func<object>, Type> registration, object rootTemplate = null, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null)
        {
            _rootTemplate = rootTemplate;
            Registration = registration;
            if (onInitializeEventHandler != null)
            {
                OnInitialize += onInitializeEventHandler;
            }
            if (onRenderEvenHandler != null)
            {
                OnRender += onRenderEvenHandler;
            }
        }

        public TemplateWrapper WithRootTemplate(object rootTemplate)
        {
            return new TemplateWrapper(Registration, rootTemplate, (sender, args) => OnInitialize?.Invoke(sender, args), (sender, args) => OnRender?.Invoke(sender, args));
        }

        public void Render(StringBuilder builder)
        {
            OnRender?.Invoke(this, new TemplateRenderEventArgs(this, builder));

            if (_currentInstance == null)
            {
                _currentInstance = Registration.Item2();
                _additionalActionDelegate = null;
            }

            if (Model != null)
            {
                TemplateRenderHelper.SetModelOnTemplate(_currentInstance, Model, true);
            }

            if (_rootTemplate != null)
            {
                TemplateRenderHelper.SetTemplateContextOnChildTemplate(_rootTemplate, _currentInstance, Model, ViewModel, TemplateContext);

                if (TemplateContext != null)
                {
                    TemplateRenderHelper.SetRootTemplateOnChildTemplate(_rootTemplate, _currentInstance, true);
                }
            }

            TemplateRenderHelper.RenderTemplate(_currentInstance, builder, Session, additionalActionDelegate: _additionalActionDelegate);
        }

        public void Initialize(Action additionalActionDelegate = null)
        {
            OnInitialize?.Invoke(this, new TemplateInitializeEventArgs(this));

            _currentInstance = Registration.Item2();
            _additionalActionDelegate = additionalActionDelegate;
        }
    }
}