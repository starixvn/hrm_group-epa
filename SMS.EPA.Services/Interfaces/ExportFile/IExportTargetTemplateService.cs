using System.Collections.Generic;
using SMS.EPA.Models.TargetTemplate;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportTargetTemplateService
{
    void Export(TargetTemplate targetTemplate, IEnumerable<TargetTemplateKPIGroupModel> kpis, string templatePath);
}