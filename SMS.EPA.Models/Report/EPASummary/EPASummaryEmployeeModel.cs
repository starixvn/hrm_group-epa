using System;
using System.Collections.Generic;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Models.Report.EPASummary;

public class EPASummaryEmployeeModel
{
    public int EmployeeId { get; set; }

    public string Code { get; set; }

    public string NickName { get; set; }

    public string FullName { get; set; }

    public string Section { get; set; }

    public string Department { get; set; }

    public Group Group { get; set; }

    public DateTime JoinedDate { get; set; }

    public JobTitle JobTitle { get; set; }

    public string Category { get; set; }

    public IList<EPASummaryPeriodModel> Periods { get; set; } = new List<EPASummaryPeriodModel>();
}