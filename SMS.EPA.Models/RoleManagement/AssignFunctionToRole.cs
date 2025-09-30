using System.Collections.Generic;
using GO.UserControl.Client.Core.Models;

namespace SMS.EPA.Models.RoleManagement;

public class AssignFunctionToRole
{
    public short RoleId { get; set; }
    public IList<FunctionPermission> Permissions { get; set; }
}