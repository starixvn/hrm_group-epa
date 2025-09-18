using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Services.Interfaces
{
    public interface IReportFacadeService
    {
        IReportService ReportService { get; }

        IEpaDetailReportService EpaDetailReportService { get; }

        IEpaDetailReportService MonthlyEpaDetailReportService { get; }

        IDisciplineService DisciplineService { get; }

        IEmployeeEPAService EmployeeEPAService { get; }

        ITargetEmployeeSettingService TargetEmployeeSettingService { get; }

        IExportExcelService ExportExcelService { get; }
    }
}
