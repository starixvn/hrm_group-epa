using System.Collections.Generic;

namespace SMS.EPA.Models.FlowSetting;

public class EmployeeFlowSettingModel
{
    public IEnumerable<EvaluatorInfoViewModel> DefaultFlowSettings { get; set; }

    public IEnumerable<EvaluatorInfoViewModel> PersonalFlowSettings { get; set; }
}