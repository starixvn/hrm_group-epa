using GO.UserControl.Client.Core.Models;

namespace SMS.EPA.Model;

using System.Collections.Generic;

public class AssignFunctionToRole
{
    public short RoleId { get; set; }
    public IList<FunctionPermission> Permissions { get; set; }
}