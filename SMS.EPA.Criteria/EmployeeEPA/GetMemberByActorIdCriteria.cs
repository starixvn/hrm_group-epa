using Fanex.Data.Repository;
using SMS.EPA.Core.Enumerations;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetMemberByActorIdCriteria : CriteriaBase
{
    public byte ProcessId { get; set; }

    public int ActorUserId { get; set; }

    public int? PeriodId { get; set; }

    public int? GroupId { get; set; }

    public bool? IsLocal { get; set; }

    public bool? IsMonthly { get; set; }

    public override bool IsValid() => ActorUserId > 0 && ProcessId >= (byte)ProcessType.Evaluator;

    public override string GetSettingKey() => "EPA_GetMember";
}