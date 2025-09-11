using System.Collections.Generic;
using SMS.EPA.Models.Report.EPADetail;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IEpaDetailReportService
{
    IList<string> GetColumnsName { get; }

    void Export(EPADetailModel epaDetail);

    void Render(EPADetailModel epaDetail);

    void WriteContentTable();
}