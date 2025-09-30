using System.Collections.Generic;

namespace SMS.EPA.Models.EmployeeEPA.ProbationExtendedQuestion;

public class EPAProbationExtendedQuestionModel
{
    public string QuestionContent { get; set; }

    public IEnumerable<ProbationSubQuestionModel> SubQuestions { get; set; }
}