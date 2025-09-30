using System;
using System.Collections.Generic;
using System.Linq;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Employee;
using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Criteria.EPAHistory;
using SMS.EPA.Criteria.KPI;
using SMS.EPA.Criteria.KPIGroup;
using SMS.EPA.Criteria.Period;
using SMS.EPA.Criteria.ScoreRatingType;
using SMS.EPA.Criteria.TargetTemplate;
using SMS.EPA.Criteria.TargetType;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Models.JobDescription;
using SMS.EPA.Models.KPI;
using SMS.EPA.Models.KPIGroup;
using SMS.EPA.Models.Period;
using SMS.EPA.Models.ScoreRatingType;
using SMS.EPA.Models.TargetTemplate;
using SMS.EPA.Models.TargetType;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class CommonService(
    IKPIService kpiService = null,
    IBaseService baseService = null,
    IEmployeeService employeeService = null,
    IPeriodService periodService = null)
    : ICommonService
{
    public IEnumerable<TargetTypeModel> GetTargetType(GetTargetTypeCriteria criteria)
    {
        return baseService.GetData<TargetTypeModel>(criteria);
    }

    public IEnumerable<KPIModel> GetKPI(GetKPICriteria criteria)
    {
        return kpiService.GetKPI(criteria);
    }

    public IEnumerable<KPIGroupModel> GetKPIGroup(GetKPIGroupCriteria criteria)
    {
        return baseService.GetData<KPIGroupModel>(criteria);
    }

    public Employee GetEmployeeById(int employeeId, IList<Role> roles)
    {
        return employeeService.GetEmployeeById(employeeId, roles);
    }

    public IEnumerable<Group> GetAllGroup(GetAllGroupCriteria criteria)
    {
        return baseService.GetData<Group>(criteria);
    }

    public IEnumerable<PeriodModel> GetPeriod(GetPeriodCriteria criteria)
    {
        return periodService.GetPeriod(criteria);
    }

    public IEnumerable<StandardGradeModel> GetStandardGrade()
    {
        return baseService.GetData<StandardGradeModel>(new GetStandardGradeCriteria());
    }

    public IEnumerable<PeriodModel> GetAllPeriodIncludeImport(GetAllPeriodIncludeImportCriteria criteria)
    {
        return baseService.GetData<PeriodModel>(criteria);
    }

    public IEnumerable<TargetTemplate> GetAllTargetTemplate(GetAllTargetTemplateCriteria criteria)
    {
        return baseService.GetData<TargetTemplate>(criteria);
    }

    public IEnumerable<JobTitle> GetAllJobTitle()
    {
        var criteria = new GetAllJobTitleCriteria();
        return baseService.GetData<JobTitle>(criteria);
    }

    public IEnumerable<ScoreRatingTypeModel> GetScoreRatingType(GetScoreRatingTypeCriteria criteria)
    {
        return baseService.GetData<ScoreRatingTypeModel>(criteria);
    }

    public IEnumerable<JobDescriptionGroupItem> GetJobDescriptionByEPAId(int epaId)
    {
        var criteria = new GetJobDescriptionByEPAIdCriteria { EPAId = epaId };
        (var groupItems, var items) = baseService.FetchMultiple<JobDescriptionGroupItem, JobDescriptionItem>(criteria);
        foreach (var groupitem in groupItems)
        {
            groupitem.Items = items.Where(x => x.GroupItemId == groupitem.GroupItemId).ToList();
        }

        return groupItems.OrderBy(x => x.DisplayOrder).ToList();
    }

    public string CalculateLengthOfService(DateTime? fromDate, DateTime? toDate, DateTime? militaryFromDate, DateTime? militaryToDate)
    {
        const byte HalfMonth = 15;
        const short DayOfYear = 365;
        const byte DayOfMonth = 30;
        const byte MonthOfYear = 12;
        int totalDays;
        int years;
        int months;
        int days;
        var lengOfService = string.Empty;
        if (fromDate.HasValue)
        {
            totalDays = ((TimeSpan)(toDate - fromDate)).Days;
            if (militaryToDate.HasValue && militaryFromDate.HasValue)
            {
                totalDays -= ((TimeSpan)(militaryToDate - militaryFromDate)).Days;
            }
            years = totalDays / DayOfYear;
            months = (totalDays - (years * DayOfYear)) / DayOfMonth;
            days = totalDays - (years * DayOfYear) - (months * DayOfMonth);
            if (days >= HalfMonth)
            {
                months++;
            }

            if (months >= MonthOfYear)
            {
                years++;
                months = 0;
            }

            lengOfService = $"{years} Year(s) {months} Month(s)";
        }

        return lengOfService;
    }
}