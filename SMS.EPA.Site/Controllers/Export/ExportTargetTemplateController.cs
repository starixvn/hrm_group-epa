using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.TargetTemplate;
using SMS.EPA.Models.TargetTemplate;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Site.Controllers.Export;

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

public class ExportTargetTemplateController(
    IExportTargetTemplateService exportTargetTemplateService,
    ITargetTemplateService targetTemplateService,
    IBaseService baseService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const string TargetTemplateKPIsTemplate = "ExcelTemplates/TargetTemplateKPIsTemplate.xls";

    private readonly IBaseService _baseService = baseService ?? new BaseService();
    private readonly IExportTargetTemplateService _exportTargetTemplateService = exportTargetTemplateService ?? new ExportTargetTemplateService();
    private readonly ITargetTemplateService _targetTemplateService = targetTemplateService ?? new TargetTemplateService();

    public ExportTargetTemplateController()
        : this(null, null, null, null)
    {
    }

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

        var targetTemplate = _baseService.GetDataEntity<TargetTemplate>(criteria);
        IEnumerable<TargetTemplateKPIGroupModel> kpis = _targetTemplateService.GetKPIOfTargetTemplate(kpisCriteria, false);

        _exportTargetTemplateService.Export(targetTemplate, kpis, Path.Combine(webHostEnvironment.WebRootPath, TargetTemplateKPIsTemplate));

        return new EmptyResult();
    }
}