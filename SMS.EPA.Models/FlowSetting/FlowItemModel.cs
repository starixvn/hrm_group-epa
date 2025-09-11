using System.Collections.Generic;

namespace SMS.EPA.Models.FlowSetting;

public class FlowItemModel
{
    public int Duration { get; set; }

    public bool HasSendMail { get; set; }

    public int RemindDay { get; set; }

    public bool IncludeSelfEvaluation { get; set; }

    public List<EvaluatorInfoViewModel> Employees { get; set; }
}