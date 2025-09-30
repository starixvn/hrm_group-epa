namespace SMS.EPA.Models.JobLevelStandard.Template;

public class JobLevelStandardTemplateKPI
{
    public int KPIId { get; set; }

    public string Name { get; set; }

    public int? DisplayOrder { get; set; }
    public bool IsSelected { get; set; }
    public double? Weight { get; set; }
}