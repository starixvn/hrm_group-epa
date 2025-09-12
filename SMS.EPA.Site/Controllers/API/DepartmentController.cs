using GO.UserControl.Client.Core.Session;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Services.Implement;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Site.Controllers.API;

using System.Collections.Generic;
using GO.UserControl.Client.Core.Enumerables;
using Core.Attribute;
using Core.Constants;

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