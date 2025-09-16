using System.Collections.Generic;
using System.Threading.Tasks;
using SMS.EPA.Models.TargetTemplate;

namespace SMS.EPA.Services.Interfaces.ExportFile;

public interface IExportTargetTemplateService
{
    Task Export(TargetTemplate targetTemplate, IEnumerable<TargetTemplateKPIGroupModel> kpis, string templatePath);
}