namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Session;
using System.Linq;
using System.Web.Http;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Model;
using SMS.EPA.Core.Services;
using SMS.EPA.Criteria;
using SMS.EPA.Model;
using SMS.EPA.Services;

public class PersonalFlowController(
    IUserControlSession userControlSession,
    IPersonalFlowService personalFlowService,
    IBaseService baseService)
    : BaseController(userControlSession)
{
    private readonly IPersonalFlowService personalFlowService = personalFlowService ?? new PersonalFlowService();
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public PersonalFlowController() : this(null, null, null)
    {
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]

    public IEnumerable<PersonalFlowModel> GetByFlowTemplateId(int flowTemplateId)
    {
        return personalFlowService.GetByFlowTemplateId(flowTemplateId);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]

    public IEnumerable<FlowModel> GetById(int employeeId, int flowTemplateId)
    {
        var criteria = new GetPersonalFlowByIdCriteria
        {
            EmployeeId = employeeId,
            FlowTemplateId = flowTemplateId
        };
        return personalFlowService.GetById(criteria);
    }

    [HttpPost]
    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]
    public CommandResult CreatePersonalFlow(CreatePersonalFlowCriteria criteria)
    {
        var commandResult = new CommandResult();
        if (criteria != null)
        {
            criteria.ModifiedBy = CurrentEmployeeId;
            commandResult = baseService.ExecuteNonQuery(criteria);
        }

        return commandResult;
    }


    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Host)]


    public CommandResult DeletePersonalFlow(int employeeId, int flowTemplateId)
    {
        var criteria = new DeletePersonalFlowCriteria
        {
            EmployeeId = employeeId,
            FlowTemplateId = flowTemplateId,
            ModifiedBy = CurrentEmployeeId
        };
        return baseService.ExecuteNonQuery(criteria);
    }

    [ApiAuthorize(FunctionKey = EPAFunctionKey.FlowManagement, Policy = Policy.Read)]

    public EmployeeFlowSettingModel GetEmployeeFlowSetting(
        int flowTemplateId,
        string teamIds = null,
        bool? isMonthly = null)
    {
        var criteria = new GetEmployeeFlowSettingCriteria
        {
            FlowTemplateId = flowTemplateId,
            TeamIds = teamIds,
            IsMonthly = isMonthly
        };

        var result = new EmployeeFlowSettingModel();
        var data = baseService.GetData<EvaluatorInfoViewModel>(criteria).OrderBy(i => i.NickName);
        result.DefaultFlowSettings = data.Where(x => !x.IsExistsSetting);
        result.PersonalFlowSettings = data.Where(x => x.IsExistsSetting);

        return result;
    }
}