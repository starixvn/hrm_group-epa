namespace SMS.EPA.Site.Controllers.Export;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces.ExportFile;
using SMS.EPA.Criteria.JobLevelStandard.Period;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

public class ExportJLSPeriodEmployeeSettingController(
    IJobLevelStandardPeriodService periodService,
    IExportExcelService exportExcelService,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    private const int DefaultContentRowStartIndexForTemplate = 3;
    private const string EmployeeSettingJLSPeriodTemplate = "ExcelTemplates/EmployeeSettingJLSPeriodTemplate.xls";

    public async Task<ActionResult> ExportExcel(GetPeriodEmployeeSettingCriteria criteria)
    {
        var employees = periodService.GetEmployeeSettings(criteria);

        exportExcelService.FileName = $"{criteria.PeriodName}_Employee_Setting";
        exportExcelService.SheetName = "Employee Setting";
        exportExcelService.Title = $"{criteria.PeriodName}- Employee Setting";
        exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.ContentRootPath, EmployeeSettingJLSPeriodTemplate));

        await exportExcelService.ExportWithTemplate(employees, true, true);

        return new EmptyResult();
    }
}