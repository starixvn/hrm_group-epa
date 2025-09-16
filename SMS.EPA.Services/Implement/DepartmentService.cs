using System.Collections.Generic;
using System.Linq;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Criteria.Department;
using SMS.EPA.Models.Enitites;
using SMS.EPA.Services.Interfaces;

namespace SMS.EPA.Services.Implement;

public class DepartmentService(IBaseService baseService = null) : IDepartmentService
{
    private readonly IBaseService baseService = baseService ?? new BaseService();

    public IList<MultiSelectItem> GetOrgChart()
    {
        var criteria = new GetOrgChartCriteria();
        var (units, departments, teams) = baseService.FetchMultiple<Unit, Department, Team>(criteria);

        return GetOrgChart(units, departments, teams);
    }

    private IList<MultiSelectItem> GetOrgChart(IEnumerable<Unit> units, IEnumerable<Department> departments, IEnumerable<Team> teams)
    {
        departments = departments.Where(d => teams.Any(t => t.DepartmentId == d.DepartmentId));
        units = units.Where(u => departments.Any(d => d.UnitId == u.UnitId)).ToList();

        var orgCharts = units.Select(u => new MultiSelectItem
        {
            Id = u.UnitId,
            Text = u.Code,
            Items = departments.Where(d => d.UnitId == u.UnitId).Select(d => new MultiSelectItem
            {
                Id = d.DepartmentId,
                Text = d.Name,
                ParentId = u.UnitId,
                Items = teams.Where(team => team.DepartmentId == d.DepartmentId).Select(team => new MultiSelectItem
                {
                    Id = team.TeamId,
                    Text = team.Name,
                    IsLeaf = true,
                    ParentId = d.DepartmentId,
                }).ToList()
            }).ToList()
        });

        return orgCharts.ToList();
    }
}