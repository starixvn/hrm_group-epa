using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Period;

public class AddEmployeeToPeriodCriteria : CriteriaBase
{
    public int PeriodId { get; set; }
    public string TeamIds { get; set; }
    public int? EmployeeId { get; set; }
    public int ModifiedBy { get; set; }

    public override bool IsValid() => PeriodId > 0 && ModifiedBy > 0;

    public override string GetSettingKey() => "EpaPeriod_AddEmployee";
}