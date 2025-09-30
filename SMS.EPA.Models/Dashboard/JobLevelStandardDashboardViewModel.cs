using System.Collections.Generic;
using System.Linq;

namespace SMS.EPA.Models.Dashboard;

public class JobLevelStandardDashboardViewModel
{
    public IEnumerable<JobLevelStandardDashboard> ToDoList { get; set; } = new List<JobLevelStandardDashboard>();

    public bool HasEvaluator => ToDoList.Any(x => x.UnfinishedByEvaluator > 0 || x.FinishedByEvaluator > 0);

    public bool HasManagement => ToDoList.Any(x => x.UnfinishedByManagement > 0 || x.FinishedByManagement > 0);
}