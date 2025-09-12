using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.TargetTemplate;
using SMS.EPA.Models.TargetTemplate;
using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

public class ExportTargetTemplateController(
    IExportTargetTemplateService exportTargetTemplateService,
    ITargetTemplateService targetTemplateService,
    IBaseService baseService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const string TargetTemplateKPIsTemplate = "ExcelTemplates/TargetTemplateKPIsTemplate.xls";

    public ActionResult ExportTargetTemplateKPIs(int targetTemplateId)
    {
        var criteria = new GetTargetTemplateByIdCriteria
        {
            TargetTemplateId = targetTemplateId
        };

        var kpisCriteria = new GetKPIOfTargetTemplateCriteria
        {
            TargetTemplateId = targetTemplateId
        };

        var targetTemplate = baseService.GetDataEntity<TargetTemplate>(criteria);
        var kpis = targetTemplateService.GetKPIOfTargetTemplate(kpisCriteria, false);

        exportTargetTemplateService.Export(targetTemplate, kpis, Path.Combine(webHostEnvironment.WebRootPath, TargetTemplateKPIsTemplate));

        return new EmptyResult();
    }
}