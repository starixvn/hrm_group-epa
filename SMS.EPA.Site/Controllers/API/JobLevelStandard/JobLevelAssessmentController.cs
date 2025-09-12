using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Extensions.XML;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.JobLevelStandard.Assessment;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Models.JobLevelStandard.Assessment;
using SMS.EPA.Models.JobLevelStandard.Period;
using SMS.EPA.Services.Interfaces.JobLevelStandard;

namespace SMS.EPA.Site.Controllers.API.JobLevelStandard;

public class JobLevelAssessmentController(
    IUserControlSession userControlSession,
    IBaseService baseService,
    IJobLevelAssessmentService jobLevelAssessmentService)
    : BaseController(userControlSession)
{
    [HttpGet]
    public IList<JobLevelStandardPeriod> GetPeriods()
    {
        var criteria = new GetPeriodByActorCriteria
        {
            ActorId = CurrentEmployeeId
        };
        var periods = baseService.GetData<JobLevelStandardPeriod>(criteria);
        return periods.ToList();
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelAssessment, Policy = Policy.Read)]
    public object GetAssessments(GetJobLevelAssessmentByActorIdCriteria criteria)
    {
        var today = DateTimeOffset.Now.Date;
        var jobLevelSettings = jobLevelAssessmentService.GetJobLevelSettingByPeriod(criteria.PeriodId);

        criteria.ActorId = CurrentEmployeeId;
        var assessments = jobLevelAssessmentService.GetJobLevelAssessmentByActor(criteria);

        foreach (var assessment in assessments)
        {
            var currentProcess = assessment.Processes
                .FirstOrDefault(x => x.EvaluatorId == CurrentEmployeeId && x.FromDate <= today && x.ToDate >= today);
            if (currentProcess != null)
            {
                assessment.IsHost = GetPermissionOnFunction(EPAFunctionKey.JobLevelAssessment) == (byte)Policy.Host;
                assessment.CurrentProcessId = currentProcess.ProcessId;
            }
        }

        IList<JobLevelAssessmentKPI> kpis = assessments
            .SelectMany(ass => ass.KPIs)
            .GroupBy(kpi => new { kpi.KPIName, kpi.KPIId })
            .Select(kpi => kpi.First())
            .OrderBy(kpi => kpi.KPIId)
            .ToList();

        return new { kpis, jobLevelSettings, assessments };
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelAssessment, Policy = Policy.Host)]
    public CommandResult Edit(JobLevelAssessment assessment)
    {
        var commandResult = new CommandResult();
        if (assessment != null)
        {
            var command = new EditJobLevelAssessmentCommand
            {
                PeriodId = assessment.PeriodId,
                EmployeeId = assessment.EmployeeId,
                TotalWeight = assessment.TotalWeight,
                CurrentJobLevelId = assessment.CurrentJobLevelId,
                NewJobLevelId = assessment.NewJobLevelId,
                StatusId = assessment.StatusId,
                AssessmentKPIXml = assessment.KPIs.ToXML().ToString(),
                Remark = assessment.Remark,
                ModifiedBy = CurrentEmployeeId
            };
            commandResult = baseService.ExecuteNonQuery(command);
            commandResult.ObjectId = command.AssessmentId;
        }

        return commandResult;
    }

    [HttpPut]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.JobLevelAssessment, Policy = Policy.Host)]
    public CommandResult EditStatus(EditStatusJobLevelAssessmentCommand command)
    {
        var commandResult = new CommandResult();
        if (command != null)
        {
            command.ModifiedBy = CurrentEmployeeId;
            commandResult = baseService.ExecuteNonQuery(command);
        }

        return commandResult;
    }

    [HttpGet]
    public IList<MultiSelectItem> GetOrgChart(int? periodId = null)
    {
        var criteria = new GetOrgChartByActorIdCriteria { ActorId = CurrentEmployeeId, PeriodId = periodId };
        (var departments, var sections, _) = baseService.FetchMultiple<Department, Section, Team>(criteria);

        var items = departments.Any()
            ? departments.Select(dept => new MultiSelectItem
            {
                Id = dept.DepartmentId,
                Text = dept.Name,
                Items = sections.Where(s => s.DepartmentId.HasValue && s.DepartmentId.Value == dept.DepartmentId).Select(s => new MultiSelectItem
                {
                    Id = s.SectionId,
                    Text = s.Name,
                    ParentId = dept.DepartmentId,
                    IsLeaf = true
                }).ToList()
            }).ToList()
            : sections.Select(s => new MultiSelectItem
            {
                Id = s.SectionId,
                Text = s.Name,
                IsLeaf = true
            }).ToList();

        return items;
    }
}