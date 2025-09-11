using SMS.EPA.Models.Report.EPADetail;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IMonthlyEpaDetailReportService
{
    void Export(EPADetailModel epaDetail);

    void Render(EPADetailModel epaDetail);
}