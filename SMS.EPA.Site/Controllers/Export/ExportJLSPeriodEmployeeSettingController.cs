
using SMS.EPA.Criteria.JobLevelStandard.Period;
using SMS.EPA.Services.Implement.JobLevelStandard;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Implement.ExportFile;
using Services.Interfaces.ExportFile;

public class ExportJLSPeriodEmployeeSettingController(
    IJobLevelStandardPeriodService periodService,
    IExportExcelService exportExcelService,
    IWebHostEnvironment webHostEnvironment)
    : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;
    private const string EmployeeSettingJLSPeriodTemplate = "ExcelTemplates/EmployeeSettingJLSPeriodTemplate.xls";
    private readonly IJobLevelStandardPeriodService periodService = periodService ?? new JobLevelStandardPeriodService();
    private readonly IExportExcelService exportExcelService = exportExcelService ?? new ExportExcelService();

    public ActionResult ExportExcel(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employees = periodService.GetEmployeeSettings(criteria);

        exportExcelService.FileName = $"{criteria.PeriodName}_Employee_Setting";
        exportExcelService.SheetName = "Employee Setting";
        exportExcelService.Title = $"{criteria.PeriodName}- Employee Setting";
        exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.WebRootPath, EmployeeSettingJLSPeriodTemplate));

        exportExcelService.ExportWithTemplate(employees, true, true);

        return new EmptyResult();
    }
}