using GO.UserControl.Client.Core.Session;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Enumerables;
using SMS.EPA.Core.Attribute;
using SMS.EPA.Core.Constants;
using SMS.EPA.Model.Enitites;
using SMS.EPA.Services;

public class DepartmentController(IUserControlSession userControlSession, IUnitService unitService)
    : BaseController(userControlSession)
{
    private readonly IUnitService unitService = unitService ?? new UnitService();


    [ApiAuthorize(FunctionKey = EPAFunctionKey.Department, Policy = Policy.Read)]
    public IList<MultiSelectItem> GetDepartment()
    {
        return unitService.GetOrgChart();
    }
}