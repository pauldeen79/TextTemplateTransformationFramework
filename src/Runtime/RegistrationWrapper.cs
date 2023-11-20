using System;
using System.Collections.Generic;
using System.Linq;

namespace TextTemplateTransformationFramework.Runtime
{
    public static class RegistrationWrapper
    {
        public static void WrapChildTemplates(object rootTemplate)
        {
            if (rootTemplate is null)
            {
                throw new ArgumentNullException(nameof(rootTemplate));
            }

            var childTemplatesField = rootTemplate.GetType().GetField("_childTemplatesField", Constants.BindingFlags);
            if (childTemplatesField is null)
            {
                throw new InvalidOperationException($"Template of type [{rootTemplate.GetType().FullName}] doesn't have a _childTemplatesField field, and cannot be mocked.");
            }

            if (childTemplatesField.GetValue(rootTemplate) is not List<Tuple<string, Func<object>, Type>> childTemplatesValue)
            {
                throw new InvalidOperationException($"Template of type [{rootTemplate.GetType().FullName}] returned null on _childTemplatesField field, and cannot be mocked. Make sure the template is already initialized, before adding mocks.");
            }

            var copy = new List<Tuple<string, Func<object>, Type>>();
            foreach (var childTemplate in childTemplatesValue)
            {
                copy.Add(WrapTemplate(childTemplate, rootTemplate));
            }

            childTemplatesValue.Clear();
            childTemplatesValue.AddRange(copy);
        }

        public static Tuple<string, Func<object>, Type> WrapTemplate(Tuple<string, Func<object>, Type> registration)
            => WrapTemplate(registration, null);

        public static Tuple<string, Func<object>, Type> WrapTemplate(Tuple<string, Func<object>, Type> registration, object rootTemplate)
        {
            if (registration is null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            return new Tuple<string, Func<object>, Type>(registration.Item1, () => new TemplateWrapper(registration, rootTemplate), registration.Item3);
        }

        public static Tuple<string, Func<object>, Type> WrapTemplate(string templateName, Func<object> templateCreationDelegate, Type modelType, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null)
            => new Tuple<string, Func<object>, Type>(templateName, () => new TemplateWrapper(templateName, templateCreationDelegate, modelType, null, onInitializeEventHandler, onRenderEvenHandler), modelType);

        public static Tuple<string, Func<object>, Type> WrapTemplate<T>(string templateName, Type modelType, Func<object> templateFactory = null, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null) where T : new()
            => WrapTemplate(templateName, () => templateFactory is null ? new T() : templateFactory.Invoke(), modelType, onInitializeEventHandler, onRenderEvenHandler);

        public static Tuple<string, Func<object>, Type> WrapTemplate<T>(Func<object> templateFactory, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null) where T : new()
            => WrapTemplate<T>(new T().GetType().FullName, null, templateFactory, onInitializeEventHandler, onRenderEvenHandler);

        public static Tuple<string, Func<object>, Type> WrapTemplate<T>(object templateInstance, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null) where T : new()
            => WrapTemplate<T>(templateInstance is null ? null : new Func<object>(() => templateInstance), onInitializeEventHandler, onRenderEvenHandler);

        public static Tuple<string, Func<object>, Type> WrapTemplate<T>(EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null) where T : new()
            => WrapTemplate<T>((object)null, onInitializeEventHandler, onRenderEvenHandler);

        public static Tuple<string, Func<object>, Type> WrapTemplate(string templateName, object templateInstance, Type modelType, EventHandler<TemplateInitializeEventArgs> onInitializeEventHandler = null, EventHandler<TemplateRenderEventArgs> onRenderEvenHandler = null)
            => new Tuple<string, Func<object>, Type>(templateName, () => new TemplateWrapper(templateName, templateInstance, modelType, null, onInitializeEventHandler, onRenderEvenHandler), modelType);

        public static Tuple<string, Func<object>, Type> WrapViewModel(string viewModelName, Func<object> viewModelCreationDelegate, Type modelType)
            => new Tuple<string, Func<object>, Type>(viewModelName, viewModelCreationDelegate, modelType);

        public static Tuple<string, Func<object>, Type> WrapViewModel<T>(string viewModelName, Type modelType, Func<object> viewModelFactory = null) where T : new()
            => WrapViewModel(viewModelName, () => viewModelFactory is null ? new T() : viewModelFactory.Invoke(), modelType);

        public static Tuple<string, Func<object>, Type> WrapViewModel<T>(Func<object> viewModelFactory) where T : new()
            => WrapViewModel<T>(new T().GetType().FullName, null, viewModelFactory);

        public static Tuple<string, Func<object>, Type> WrapViewModel<T>(object viewModelInstance) where T : new()
            => WrapViewModel<T>(viewModelInstance is null ? null : new Func<object>(() => viewModelInstance));

        public static Tuple<string, Func<object>, Type> WrapViewModel<T>() where T : new()
            => WrapViewModel<T>((object)null);

        public static Tuple<string, Func<object>, Type> WrapViewModel(string viewModelName, object viewModelInstance, Type modelType)
            => new Tuple<string, Func<object>, Type>(viewModelName, () => viewModelInstance, modelType);

        public static Tuple<string, Func<object>, Type> Resolve(Tuple<string, Func<object>, Type> registration, IEnumerable<Tuple<string, Func<object>, Type>> registrations)
            => registrations.FirstOrDefault(wrapper => wrapper.Item1 == registration.Item2().GetType().FullName);

        public static IEnumerable<Tuple<string, Func<object>, Type>> Create(params Tuple<string, Func<object>, Type>[] registrations)
            => registrations;
    }
}
