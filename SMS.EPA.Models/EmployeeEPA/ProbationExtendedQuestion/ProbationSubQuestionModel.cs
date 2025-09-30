namespace SMS.EPA.Models.EmployeeEPA.ProbationExtendedQuestion;

public class ProbationSubQuestionModel
{
    public int EPAId { get; set; }
    public int SubQuestionId { get; set; }
    public string SubQuestionContent { get; set; }
    public bool? IsAgree { get; set; }
}