using System.Collections.Generic;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Models.Dashboard;

public class DashboardModel
{
    public Enitites.Employee Employee { get; set; }

    public IEnumerable<EPAModel> MyEpaInfos { get; set; }

    public IEnumerable<EPAModel> MonthlyEpaInfos { get; set; }
}