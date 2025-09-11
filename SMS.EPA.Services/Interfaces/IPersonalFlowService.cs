using System.Collections.Generic;
using SMS.EPA.Criteria.FlowSetting;
using SMS.EPA.Models.FlowSetting;

namespace SMS.EPA.Services.Interfaces;

public interface IPersonalFlowService
{
    IEnumerable<PersonalFlowModel> GetByFlowTemplateId(int flowTemplateId);

    IEnumerable<FlowModel> GetById(GetPersonalFlowByIdCriteria criteria);
}