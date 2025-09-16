using System.Threading.Tasks;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.TemplateSetting;

namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Core.Services;
using Criteria.JobLevelStandard.TemplateSetting;
using Services.Implement.ExportFile;
using Services.Interfaces.ExportFile;

public class ExportJLSTemplateSettingController(
    IExportExcelService exportExcelService,
    IBaseService baseService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;
    private const string Template = "App_Data/ExcelTemplates/JLSTeamTemplateSetting.xls";

    private readonly IBaseService _baseService = baseService ?? new BaseService();
    private readonly IExportExcelService _exportExcelService = exportExcelService ?? new ExportExcelService();


    public async Task<ActionResult> ExportSectionTemplateSetting(GetJobLevelStandardTemplateSettingCriteria criteria)
    {
        var datas = _baseService.GetData<JobLevelStandardTemplateSetting>(criteria);

        _exportExcelService.FileName = "JLS_Team_Template_Setting";
        _exportExcelService.SheetName = "Team";
        _exportExcelService.Title = "JLS Template Setting";

        _exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, Template));
        _exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        await _exportExcelService.ExportWithTemplate(datas, true, true);

        return new EmptyResult();
    }
}