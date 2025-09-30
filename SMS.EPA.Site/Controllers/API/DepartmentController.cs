namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using Core.Attribute;
using Core.Constants;
using GO.UserControl.Client.Core.Enumerables;
using GO.UserControl.Client.Core.Session;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Services.Interfaces;

public class DepartmentController(
    IUserControlSession session,
    IUnitService unitService) : BaseController(session)
{
    [ApiAuthorize(FunctionKey = EPAFunctionKey.Department, Policy = Policy.Read)]
    public IList<MultiSelectItem> GetDepartment()
    {
        return unitService.GetOrgChart();
    }
}