using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateInfoRepository
    {
        IEnumerable<TemplateInfo> GetTemplates();
        void Add(TemplateInfo templateInfo);
        void Update(TemplateInfo templateInfo);
        void Remove(TemplateInfo templateInfo);
    }
}
