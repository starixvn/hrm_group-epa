using System;

namespace SMS.EPA.Models.Enitites;

[Serializable]
public class Unit
{
    public int UnitId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}