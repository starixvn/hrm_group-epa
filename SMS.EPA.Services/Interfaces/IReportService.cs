using System.Collections.Generic;
using System.IO;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Criteria.Report;
using SMS.EPA.Models.Dashboard;
using SMS.EPA.Models.Employee;
using SMS.EPA.Models.Period;
using SMS.EPA.Models.Report.EPADetail;
using SMS.EPA.Models.Report.EPASummary;

namespace SMS.EPA.Services.Interfaces;

public interface IReportService
{
    DashboardModel GetDashboard(int employeeId);

    EPASummaryModel GetEPASummaryReport(GetEPASummaryCriteria criteria);

    MemoryStream ExportEPASummaryReport(EPASummaryModel epaSummary);

    MemoryStream ExportMonthlyEPASummaryReport(EPASummaryModel epaSummary);

    EPADetailModel GetEPADetailReport(GetEPADetailCriteria criteria, int currentEmployeeId, IList<Role> roles);

    PeriodModel GetEPADetailPeriodForExportedFileName(string periodId);

    EPADetailModel GetMonthlyEPADetailReport(GetMonthlyEPADetailCriteria criteria, int currentEmployeeId, IList<Role> roles);

    EPASummaryModel GetMonthlyEPASummaryReport(GetMonthlyEPASummaryCriteria criteria);

    IEnumerable<EmployeeViewModel> GetEmployeeByActorId(GetEmployeeByActorIdCriteria criteria);
}