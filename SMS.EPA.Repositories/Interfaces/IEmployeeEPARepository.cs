using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.Dashboard;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA.RatingByKPI;

namespace SMS.EPA.Repositories.Interfaces;

public interface IEmployeeEPARepository
{
    IEnumerable<EPAModel> GetEPA(ICriteria criteria);

    EPAModel GetEPAInfo(ICriteria criteria);

    IEnumerable<EPAModel> GetEPAEndEvaluationPeriod(ICriteria criteria);

    IEnumerable<TemporaryKPIModel> GetKPIByTargetTemplate(ICriteria criteria);

    IEnumerable<RatingByKPIModel> GetEmployeeEPAByKPI(ICriteria criteria);

    DashboardModel GetDashboard(ICriteria criteria);

    IList<EPAModel> GetEPAToUpdateFinalScore(ICriteria criteria);
}