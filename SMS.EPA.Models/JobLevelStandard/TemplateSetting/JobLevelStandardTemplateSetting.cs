using SMS.EPA.Models.Report;

namespace SMS.EPA.Models.JobLevelStandard.TemplateSetting;

public class JobLevelStandardTemplateSetting : IExcelRecord
{
    public int ObjectId { get; set; }
    public byte ObjectTypeId { get; set; }
    public string UnitName { get; set; }
    public string DepartmentName { get; set; }
    public string TeamName { get; set; }
    public string CompanyName { get; set; }
    public string Template { get; set; }

    public object ConvertToExcelRecord() => new
    {
        No = 0,
        UnitName,
        DepartmentName,
        TeamName,
        CompanyName,
        Template
    };
}