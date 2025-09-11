using System.Collections.Generic;
using System.Linq;
using Fanex.Data;
using Fanex.Data.Repository;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.EPAHistory;
using SMS.EPA.Repositories.Interfaces;

namespace SMS.EPA.Repositories.Implement;

public class EPAHistoryRepository : IEPAHistoryRepository
{
    public IList<EPAHistoryModel> GetEPAHistory(ICriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            return db.Query<EPAHistoryModel, EPAPeriodModel, EPAEmployeeInfo, EPAHistoryModel>(
                (epa, pe, emp) =>
                {
                    epa.Employee = emp;
                    epa.Period = pe;
                    return epa;
                },
                "PeriodId,EmployeeId",
                criteria).ToList();
        }
    }

    public IList<EPAHistoryModel> GetMonthlyEPAHistory(ICriteria criteria)
    {
        using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
        {
            return db.Query<EPAHistoryModel, EPAPeriodModel, EPAEmployeeInfo, EPAHistoryModel>(
                (epa, pe, emp) =>
                {
                    epa.Employee = emp;
                    epa.Period = pe;
                    return epa;
                },
                "PeriodId,EmployeeId",
                criteria).ToList();
        }
    }
}