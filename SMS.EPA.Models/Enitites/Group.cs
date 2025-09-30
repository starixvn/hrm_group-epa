using System;

namespace SMS.EPA.Models.Enitites;

[Serializable]
public class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public string GroupCode { get; set; }
}