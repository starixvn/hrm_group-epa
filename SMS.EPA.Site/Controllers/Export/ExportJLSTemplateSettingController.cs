using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.TemplateSetting;

namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria.JobLevelStandard.TemplateSetting;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Interfaces.ExportFile;

public class ExportJLSTemplateSettingController(
    IExportExcelService exportExcelService,
    IBaseService baseService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;
    private const string Template = "ExcelTemplates/JLSSectionTemplateSetting.xls";

    private readonly IBaseService _baseService = baseService ?? new BaseService();
    private readonly IExportExcelService _exportExcelService = exportExcelService ?? new ExportExcelService();

    public ExportJLSTemplateSettingController() : this(null, null, null)
    {
    }

    public ActionResult ExportSectionTemplateSetting(GetJobLevelStandardTemplateSettingCriteria criteria)
    {
        var datas = _baseService.GetData<JobLevelStandardTemplateSetting>(criteria);

        _exportExcelService.FileName = "JLS_Section_Template_Setting";
        _exportExcelService.SheetName = "Section";
        _exportExcelService.Title = "JLS Template Setting";

        _exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.WebRootPath, Template));
        _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        _exportExcelService.ExportWithTemplate(datas, true, true);

        return new EmptyResult();
    }
}