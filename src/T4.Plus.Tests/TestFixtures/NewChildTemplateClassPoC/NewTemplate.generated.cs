﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 5.0.13
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace NewTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TextTemplateTransformationFramework.Runtime;

    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public partial class GeneratedTemplate : GeneratedTemplateBase
    {
        public virtual void Render(global::System.Text.StringBuilder builder)
        {
            var backup = this.GenerationEnvironment;
            if (builder != null) this.GenerationEnvironment = builder;

            RenderChildTemplate(@"Child");


            if (builder != null) this.GenerationEnvironment = backup;
        }



        public virtual void Initialize()
        {
            this.Errors.Clear();
            this.GenerationEnvironment.Clear();
            if (Session == null)
            {
                Session = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            RegisterChildTemplate(@"Child", () => new Child());

        }

    }

    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public partial class GeneratedTemplateBase : TextTemplateTransformationFramework.Runtime.T4PlusGeneratedTemplateBase
    {


    }
    #endregion
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public class GeneratedTemplateBaseChild : GeneratedTemplateBase
    {
        public GeneratedTemplate RootTemplate { get; set; }

        public override void Write(string textToAppend)
        {
            if (RootTemplate != null)
            {
                RootTemplate.Write(textToAppend);
            }
            else
            {
                base.Write(textToAppend);
            }
        }

        public override void WriteLine(string textToAppend)
        {
            if (RootTemplate != null)
            {
                RootTemplate.WriteLine(textToAppend);
            }
            else
            {
                base.WriteLine(textToAppend);
            }
        }
    }
    [System.CodeDom.Compiler.GeneratedCodeAttribute(@"T4PlusCSharpCodeGenerator", @"1.0.0.0")]
    public class Child : GeneratedTemplateBaseChild
    {
        public virtual void Render(global::System.Text.StringBuilder builder)
        {
            var backup = this.GenerationEnvironment;
            if (builder != null) this.GenerationEnvironment = builder;
            Write(this.ToStringHelper.ToStringWithCulture(@"Hello world!"));

            if (builder != null) this.GenerationEnvironment = backup;
        }


        public virtual void Initialize(global::System.Action additionalActionDelegate = null)
        {
            this.Errors.Clear();
            this.GenerationEnvironment.Clear();
            if (Session == null)
            {
                Session = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            if (RootTemplate != null)
            {
                ChildTemplates = RootTemplate.ChildTemplates;
                ViewModels = RootTemplate.ViewModels;
            }
            else
            {
                ChildTemplates.Clear();
                ViewModels.Clear();
            }
            if (RootTemplate != null)
            {
                PlaceholderChildrenDictionary = RootTemplate.PlaceholderChildrenDictionary;
            }
            else
            {
                PlaceholderChildrenDictionary.Clear();
            }

        }


    }

}