
using SMS.EPA.Criteria.JobLevelStandard.Period;
using SMS.EPA.Models.JobLevelStandard.Period;
using SMS.EPA.Services.Implement.JobLevelStandard;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Site.Controllers.Export;

using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SMS.EPA.Services;
using SMS.EPA.Services.Implement.ExportFile;
using SMS.EPA.Services.Interfaces.ExportFile;

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
        IList<PeriodEmployeeSetting> employees = periodService.GetEmployeeSettings(criteria);

        exportExcelService.FileName = string.Format("{0}_Employee_Setting", criteria.PeriodName);
        exportExcelService.SheetName = "Employee Setting";
        exportExcelService.Title = string.Format("{0}- Employee Setting", criteria.PeriodName);
        exportExcelService.ContentRowStartIndex = DefaultContentRowStartIndexForTemplate;

        exportExcelService.InitializeWithTemplate(Path.Combine(webHostEnvironment.WebRootPath, EmployeeSettingJLSPeriodTemplate));

        exportExcelService.ExportWithTemplate(employees, true, true);

        return new EmptyResult();
    }
}