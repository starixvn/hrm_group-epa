using SMS.EPA.Criteria.TargetTemplate;
using SMS.EPA.Models.KPIGroup;
using SMS.EPA.Models.TargetTemplate;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;

public interface ITargetTemplateService
{
    IEnumerable<KPIGroupModel> GetKPIGroupOfTargetTemplate(GetKPIGroupOfTargetTemplateCriteria criteria);

    IEnumerable<TargetTemplateKPIGroupModel> GetKPIOfTargetTemplate(GetKPIOfTargetTemplateCriteria criteria, bool onlyKPISelected);
}