using Fanex.Data.Repository;

namespace SMS.EPA.Criteria.EmployeeEPA;

public class GetEPASummaryImportCriteria : CriteriaBase
{
    public override bool IsValid()
    {
        return true;
    }

    public override string GetSettingKey()
    {
        return "EPASummaryImport_Get";
    }
}