using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetExtendedQuestionCriteria : CriteriaBase
{
    public int EPAId { get; set; }

    public override string GetSettingKey()
    {
        return "EPA_GetExtendedQuestion";
    }

    public override bool IsValid()
    {
        return EPAId > 0;
    }
}