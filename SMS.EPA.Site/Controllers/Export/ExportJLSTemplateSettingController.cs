namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using System.Threading.Tasks;
using Criteria.JobLevelStandard.TemplateSetting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.ExportFile;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.JobLevelStandard.TemplateSetting;

public class ExportJLSTemplateSettingController(
    IExportExcelService exportExcelService,
    IBaseService baseService,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;
    private const string Template = "ExcelTemplates/JLSTeamTemplateSetting.xls";

    public async Task<ActionResult> ExportSectionTemplateSetting(GetJobLevelStandardTemplateSettingCriteria criteria)
    {
        var data = baseService.GetData<JobLevelStandardTemplateSetting>(criteria);

        exportExcelService.FileName = "JLS_Team_Template_Setting";
        exportExcelService.SheetName = "Team";
        exportExcelService.Title = "JLS Template Setting";

        exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, Template));
        exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        await exportExcelService.ExportWithTemplate(data, true, true);

        return new EmptyResult();
    }
}