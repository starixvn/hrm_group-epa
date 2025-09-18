using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.EPAHistory;

namespace SMS.EPA.Repositories.Interfaces;

public interface IEPAHistoryRepository
{
    IList<EPAHistoryModel> GetEPAHistory(ICriteria criteria);
}