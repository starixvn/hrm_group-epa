using System;
using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.Discipline;

public class GetDisciplineSummaryCriteria : CriteriaBase
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ApproverId { get; set; }
    public string TeamIds { get; set; }
    public bool? IsLocal { get; set; }
    public int? EmployeeId { get; set; }
    public short? ReasonId { get; set; }
    public bool IsShowAll { get; set; }

    public override bool IsValid() => EndDate >= StartDate && ApproverId > 0;

    public override string GetSettingKey() => "DisciplineSummary_Get";
}