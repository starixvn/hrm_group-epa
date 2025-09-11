using System.Collections.Generic;
using System.Linq;
using SMS.EPA.AutoService.Criteria;
using SMS.EPA.AutoService.Service.Interfaces;
using SMS.EPA.Core.Extensions.XML;
using SMS.EPA.Core.Services;
using SMS.EPA.Core.Services.Interfaces;
using SMS.EPA.Models.AutoService;
using SMS.EPA.Services.Implement;

namespace SMS.EPA.AutoService.Service.Implement;

public class ProcessService(
    ICryptographyService cryptographyService,
    IBaseService baseService,
    IEPAProcessService processService)
    : IProcessService
{
    private readonly ICryptographyService cryptographyService = cryptographyService ?? new CryptographyService();
    private readonly IBaseService baseService = baseService ?? new BaseService();
    private readonly IEPAProcessService processService = processService ?? new EPAProcessService();

    public ProcessService() : this(null, null, null)
    {
    }

    public void UpdateInfomationOfProcess()
    {
        var criteria = new CheckEndProcessCriteria();
        var processes = baseService.GetData<AutoEvaluationProcessModel>(criteria).ToList();
        DecryptTotalScore(processes);
        if (processes != null && processes.Any())
        {
            UpdateEndProcess(processes);
            processService.UpdateTotalScoreOfProcess(processes);
        }
    }

    private void UpdateEndProcess(IEnumerable<AutoEvaluationProcessModel> processes)
    {
        var criteria = new UpdateEndProcessCriteria
        {
            EPAProcess = processes.ToXML().ToString()
        };
        baseService.ExecuteNonQuery(criteria);
    }

    private void DecryptTotalScore(IEnumerable<AutoEvaluationProcessModel> evaluationProcesses)
    {
        foreach (AutoEvaluationProcessModel evaluationProcess in evaluationProcesses)
        {
            evaluationProcess.TotalScore = cryptographyService.Decrypt(evaluationProcess.TotalScore);
            evaluationProcess.AdjustScore = cryptographyService.Decrypt(evaluationProcess.AdjustScore);
        }
    }
}