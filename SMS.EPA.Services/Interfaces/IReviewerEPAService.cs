using SMS.EPA.Criteria.EmployeeEPA;
using SMS.EPA.Models.EmployeeEPA;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;
using Criteria;

public interface IReviewerEPAService
{
    IEnumerable<EPAModel> GetEPAByReviewer(GetEPAByReviewerCriteria criteria);

    EPAModel GetEPAInfoByReviewer(GetEPAInfoByReviewerCriteria criteria);
}