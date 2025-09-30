using Fanex.Data.Repository;
using SMS.EPA.Models.EmployeeEPA.ProbationExtendedQuestion;

namespace SMS.EPA.Repositories.Interfaces;

public interface IProbationExtendedQuestionRepository
{
    EPAProbationExtendedQuestionModel GetExtendedQuestion(ICriteria criteria);
}