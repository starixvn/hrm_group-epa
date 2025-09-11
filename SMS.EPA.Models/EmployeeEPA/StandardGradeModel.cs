namespace SMS.EPA.Models.EmployeeEPA;

public class StandardGradeModel
{
    public int GradeId { get; set; }

    public int Type { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public float LeftScore { get; set; }

    public float RightScore { get; set; }
}