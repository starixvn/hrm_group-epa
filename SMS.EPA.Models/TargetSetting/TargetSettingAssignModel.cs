using System.Collections.Generic;
using System.Text;
using SMS.EPA.Core.Constants;
using SMS.EPA.Models.Enitites;

namespace SMS.EPA.Models.TargetSetting;

public class TargetSettingAssignModel
{
    private const string DepartmentXmlTemplate = "<Department DepartmentId='{0}' IsGroup='{1}'></Department>";

    public int TargetTemplateId { get; set; }

    public IEnumerable<Department> DepartmentSettings { get; set; }

    public string ConvertToXml()
    {
        var departmentXml = new StringBuilder(Constants.BeginRootTag);

        if (DepartmentSettings != null)
        {
            foreach (Department department in DepartmentSettings)
            {
                departmentXml.AppendFormat(DepartmentXmlTemplate, department.DepartmentId, department.IsGroup ? 1 : 0);
            }
        }

        return departmentXml.Append(Constants.EndRootTag).ToString();
    }
}