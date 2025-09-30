using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Period.GenerationEvaluation;

public class GetEmployeeSettingToSendMailCriteria : CriteriaBase
{
    public int? PeriodId { get; set; }
    public int? ProcessId { get; set; }
    public string EmployeeIds { get; set; }
    public bool? IsEvaluated { get; set; }
    public byte? MailMode { get; set; }

    public override bool IsValid() => true;

    public override string GetSettingKey() => "EpaAuto_GetEmployeeSettingToSendMail";
}