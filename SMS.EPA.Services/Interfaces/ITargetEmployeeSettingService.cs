using SMS.EPA.Criteria.TargetEmployeeSetting;
using SMS.EPA.Models.TargetSetting;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;
using Criteria;

public interface ITargetEmployeeSettingService
{
    IList<TargetEmployeeSetting> GetTargetEmployeeSetting(GetAllTargetEmployeeSettingCriteria criteria);

    IList<TargetDepartmentSetting> GetTargetDepartmentSetting(GetTargetDepartmentSettingCriteria criteria);
}