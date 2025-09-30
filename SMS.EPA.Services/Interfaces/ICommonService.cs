using System;
using System.Collections.Generic;
using GO.UserControl.Client.Core.Models.Role;
using SMS.EPA.Criteria.Employee;
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

namespace SMS.EPA.Services.Interfaces;

public interface ICommonService
{
    IEnumerable<TargetTypeModel> GetTargetType(GetTargetTypeCriteria criteria);

    IEnumerable<KPIModel> GetKPI(GetKPICriteria criteria);

    IEnumerable<KPIGroupModel> GetKPIGroup(GetKPIGroupCriteria criteria);

    Employee GetEmployeeById(int employeeId, IList<Role> roles);

    IEnumerable<Group> GetAllGroup(GetAllGroupCriteria criteria);

    IEnumerable<PeriodModel> GetPeriod(GetPeriodCriteria criteria);

    IEnumerable<StandardGradeModel> GetStandardGrade();

    IEnumerable<PeriodModel> GetAllPeriodIncludeImport(GetAllPeriodIncludeImportCriteria criteria);

    IEnumerable<TargetTemplate> GetAllTargetTemplate(GetAllTargetTemplateCriteria criteria);

    IEnumerable<JobTitle> GetAllJobTitle();

    IEnumerable<ScoreRatingTypeModel> GetScoreRatingType(GetScoreRatingTypeCriteria criteria);

    IEnumerable<JobDescriptionGroupItem> GetJobDescriptionByEPAId(int epaId);

    string CalculateLengthOfService(DateTime? fromDate, DateTime? toDate, DateTime? militaryFromDate, DateTime? militaryToDate);
}