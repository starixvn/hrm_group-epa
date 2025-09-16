using System.Collections.Generic;
using System.Linq;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.FlowSetting;
using SMS.EPA.Models.FlowSetting;
using SMS.EPA.Repositories.Implement;
using SMS.EPA.Repositories.Interfaces;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class DepartmentFlowService(
    IBaseService baseService,
    IFlowSettingRepository flowSettingRepository)
    : IDepartmentFlowService
{
    private readonly IBaseService baseService = baseService ?? new BaseService();
    private readonly IFlowSettingRepository flowSettingRepository = flowSettingRepository ?? new FlowSettingRepository();

    public DepartmentFlowService() : this(null, null)
    {
    }

    public IEnumerable<DepartmentFlowModel> GetByFlowTemplateId(int flowTemplateId)
    {
        var criteria = new GetDepartmentFlowByFlowTemplateIdCriteria
        {
            FlowTemplateId = flowTemplateId
        };
        var (flows, processes) = baseService.FetchMultiple<DepartmentFlowModel, ProcessFlowModel>(criteria);
        MappingFlow(flows, processes);
        return flows.OrderBy(i => i.Unit).ThenBy(x => x.Department);
    }

    public IEnumerable<FlowModel> GetById(GetDepartmentFlowByIdCriteria criteria)
    {
        IEnumerable<FlowModel> flow = new List<FlowModel>();
        if (criteria != null && criteria.IsValid())
        {
            flow = flowSettingRepository.GetFlowSettingByFlowTemplateId(criteria);
        }

        return flow;
    }

    private static void MappingFlow(IEnumerable<DepartmentFlowModel> flows, IEnumerable<ProcessFlowModel> processes)
    {
        foreach (var flow in flows)
        {
            foreach (var process in processes.Where(x => x.GroupId == flow.GroupId))
            {
                switch (process.ProcessId)
                {
                    case (byte)ProcessType.HumanResource:
                        flow.HumanResources.Add(process);
                        break;

                    case (byte)ProcessType.Evaluator:
                        flow.Evaluators.Add(process);
                        break;

                    case (byte)ProcessType.Reviewer:
                        flow.Reviewers.Add(process);
                        break;

                    case (byte)ProcessType.Management:
                        flow.Managements.Add(process);
                        break;

                    default:
                        continue;
                }
            }
        }
    }
}