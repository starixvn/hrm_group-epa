using System;
using GO.UserControl.Client.Core.Models;
using GO.UserControl.Client.Core.Models.Role;
using GO.UserControl.Client.Core.Services;
using GO.UserControl.Client.Core.Session;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace SMS.EPA.Site.Controllers.API;

using Core.Constants;
using Core.Enumerations;
using SMS.EPA.Core.Services;
using Core.Utilities;
using Criteria;
using Criteria.EPAHistory;
using SMS.EPA.Criteria.JobLevelStandard.Template;
using Criteria.TargetType;
using Model;
using Model.Enitites;
using SMS.EPA.Model.JobLevelStandard.Template;
using Services;

public class CommonController(
    IUserControlSession userControlSession,
    IRoleService roleService,
    IBaseService baseService,
    ICommonService commonService,
    IUnitService unitService,
    IDepartmentService departmentService)
    : BaseController(userControlSession)
{
    [HttpGet]
    public IEnumerable<TargetTypeModel> GetTargetType(
        bool? isActive = true,
        string excludeTargetTypeIds = "")
    {
        var criteria = new GetTargetTypeCriteria
        {
            IsActive = isActive,
            ExcludeTargetTypeIds = excludeTargetTypeIds
        };

        return commonService.GetTargetType(criteria);
    }

    [HttpGet]
    public IEnumerable<KPIModel> GetKPI(
        int? kpiGroupId = null,
        bool? isActive = null,
        bool? isRequiredEvaluate = null)
    {
        var criteria = new GetKPICriteria
        {
            KPIGroupId = kpiGroupId,
            IsActive = isActive,
            IsRequiredEvaluate = isRequiredEvaluate
        };

        return commonService.GetKPI(criteria);
    }

    [HttpGet]
    public IEnumerable<KPIGroupModel> GetKPIGroup(bool? isActive = null)
    {
        var criteria = new GetKPIGroupCriteria
        {
            IsActive = isActive
        };

        return commonService.GetKPIGroup(criteria);
    }

    public IList<Role> GetSystemRoles(bool? isActive = true)
    {
        IList<Role> systemRoles = roleService.GetBySiteId(userControlSession.SiteId, isActive);
        return systemRoles;
    }

    [HttpGet]
    public IList<Employee> SearchEmployee(
        string query = "",
        string teamIds = null,
        bool? isSpecialUser = null,
        bool? isDisabledUser = null,
        bool? getByApprover = null,
        bool? includeVerifier = null)
    {
        var criteria = new SearchEmployeeCriteria
        {
            Query = query,
            TeamIds = teamIds,
            IsSpecialUser = isSpecialUser,
            IsDisabledUser = isDisabledUser,
            ApproverId = getByApprover == true ? (int?)CurrentEmployeeId : null,
            IncludeVerifier = includeVerifier
        };

        return baseService.GetData<Employee>(criteria).ToList();
    }

    [HttpGet]
    public Employee GetEmployeeById(int employeeId)
    {
        IList<Role> roles = roleService.GetBySiteId(userControlSession.SiteId);
        return commonService.GetEmployeeById(employeeId, roles);
    }

    [HttpGet]
    public IEnumerable<Group> GetAllGroup()
    {
        var criteria = new GetAllGroupCriteria();

        return commonService.GetAllGroup(criteria);
    }

    public List<object> GetAllStatus()
    {
        List<object> statuses = ((EpaStatus[])Enum.GetValues(typeof(EpaStatus)))
            .Select(x => new
            {
                StatusId = (int)x,
                Name = Enumerations.GetDescription(x)
            }).Cast<object>().ToList();

        return statuses;
    }

    [HttpGet]
    public IEnumerable<PeriodModel> GetPeriod(int? targetTypeId = null, bool? isActive = true)
    {
        var criteria = new GetPeriodCriteria
        {
            TargetTypeId = targetTypeId,
            IsActive = isActive
        };

        return commonService.GetPeriod(criteria);
    }

    [HttpGet]
    public IEnumerable<StandardGradeModel> GetStandardGrade()
    {
        return commonService.GetStandardGrade();
    }

    [HttpGet]
    public IEnumerable<PeriodModel> GetAllPeriodIncludeImport()
    {
        var criteria = new GetAllPeriodIncludeImportCriteria();

        return commonService.GetAllPeriodIncludeImport(criteria);
    }

    [HttpGet]
    public IEnumerable<TargetTemplate> GetAllTargetTemplate(byte? targetTypeId = null, bool? isActive = null)
    {
        var criteria = new GetAllTargetTemplateCriteria
        {
            TargetTypeId = targetTypeId,
            IsActive = isActive
        };

        return commonService.GetAllTargetTemplate(criteria);
    }

    [HttpGet]
    public IEnumerable<MenuItem> GetMenusByEmployeeId()
    {
        try
        {
            IEnumerable<MenuItem> menus = userControlSession.MenuItems;
            if (menus == null)
            {
                return new List<MenuItem>();
            }
            return GroupMenuItem(menus);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            System.Diagnostics.Debug.WriteLine($"Error in GetMenusByEmployeeId: {ex.Message}");
            return new List<MenuItem>();
        }
    }

    public IEnumerable<JobTitle> GetAllJobTitle()
    {
        return commonService.GetAllJobTitle();
    }

    public byte GetUserPermissionOnFlowManagement()
    {
        return GetPermissionOnFunction(EPAFunctionKey.FlowManagement);
    }

    public IEnumerable<JobLevel> GetJobLevel(bool? isDeleted = null)
    {
        var criteria = new GetJobLevelCriteria { IsDeleted = isDeleted };
        return baseService.GetData<JobLevel>(criteria);
    }

    public IList<MultiSelectItem> GetOrgChart()
    {
        return departmentService.GetOrgChart();
    }

    [HttpGet]
    public IEnumerable<JobLevelStandardTemplate> GetJobLevelStandardTemplate(GetJobLevelStandardTemplateCriteria criteria)
    {
        return baseService.GetData<JobLevelStandardTemplate>(criteria);
    }

    public IList<MultiSelectItem> GetOrgChartJLSTemplateSetting()
    {
        IList<Unit> units = unitService.Get();

        var criteria = new GetOrgChartCriteria();
        (var departments, var sections) = baseService.FetchMultiple<Department, Section>(criteria);

        return GetOrgChart(units, departments.ToList(), sections.ToList());
    }

    private static IList<MenuItem> GroupMenuItem(IEnumerable<MenuItem> menuItems)
    {
        IList<MenuItem> parents = menuItems.Where(item => !item.ParentId.HasValue)
            .OrderBy(item => item.DisplayOrder).ToList();

        foreach (MenuItem parent in parents)
        {
            parent.Childs = menuItems.Where(x => x.ParentId.HasValue && x.ParentId.Value == parent.MenuItemId)
                .OrderBy(menuItem => menuItem.DisplayOrder).ToList();
        }

        return parents;
    }

    private IList<MultiSelectItem> GetOrgChart(IList<Unit> units, IList<Department> departments, IList<Section> sections)
    {
        IList<MultiSelectItem> items;
        if (units.Any())
        {
            items = units.Select(unit => new MultiSelectItem
            {
                Id = unit.UnitId,
                Text = unit.Name,
                Items = departments.Where(dept => dept.ParentId.HasValue && dept.ParentId.Value == unit.UnitId).Select(dept => new MultiSelectItem
                {
                    Id = dept.DepartmentId,
                    Text = dept.Name,
                    ParentId = unit.UnitId,
                    Items = sections.Where(s => s.DepartmentId.HasValue && s.DepartmentId.Value == dept.DepartmentId).Select(s => new MultiSelectItem
                    {
                        Id = s.SectionId,
                        Text = s.Name,
                        ParentId = dept.DepartmentId,
                        IsLeaf = true
                    }).ToList()
                }).ToList()
            }).ToList();
        }
        else if (departments.Any())
        {
            items = departments.Select(dept => new MultiSelectItem
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
            }).ToList();
        }
        else
        {
            items = sections.Select(s => new MultiSelectItem
            {
                Id = s.SectionId,
                Text = s.Name,
                IsLeaf = true
            }).ToList();
        }

        return items;
    }
}