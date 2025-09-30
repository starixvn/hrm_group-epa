using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.Period;

namespace SMS.EPA.Repositories.Interfaces;

public interface IPeriodRepository
{
    IEnumerable<PeriodModel> GetPeriod(ICriteria criteria);

    PeriodModel GetPeriodById(ICriteria criteria);

    IEnumerable<PeriodEmployeeSettingModel> GetPeriodEmployeeSettingById(ICriteria criteria);
}