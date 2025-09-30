using System.Collections.Generic;
using Fanex.Data.Repository;
using SMS.EPA.Models.KPI;

namespace SMS.EPA.Repositories.Interfaces;

public interface IKPIRepository
{
    IEnumerable<KPIModel> GetKPI(ICriteria criteria);
}