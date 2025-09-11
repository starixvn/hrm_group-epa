using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.FlowSetting;

namespace SMS.EPA.Repositories.Interfaces;

public interface IFlowSettingRepository
{
    IEnumerable<FlowModel> GetFlowSettingByFlowTemplateId(ICriteria criteria);
}