using SMS.EPA.Models.EPAHistory;

namespace SMS.EPA.Services.Interfaces;

using System.Collections.Generic;
using Criteria.EPAHistory;

public interface IEPAHistoryService
{
    IList<EPAHistoryModel> GetEPAHistory(GetEPAHistoryCriteria criteria);

    IList<EPAHistoryModel> GetMonthlyEPAHistory(GetMonthlyEPAHistoryCriteria criteria);
}