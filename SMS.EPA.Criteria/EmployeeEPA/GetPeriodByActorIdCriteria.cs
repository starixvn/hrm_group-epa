using Fanex.Data.Repository;
using SMS.EPA.Core.Enumerations;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetPeriodByActorIdCriteria : CriteriaBase
{
    public int ActorId { get; set; }

    public byte ProcessId { get; set; }

    public override bool IsValid() => ActorId > 0 && ProcessId >= (byte)ProcessType.Evaluator;

    public override string GetSettingKey() => "EPA_GetPeriodByActorId";
}