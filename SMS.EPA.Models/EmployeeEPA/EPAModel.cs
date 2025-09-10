namespace SMS.EPA.Model;

using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using Core.Enumerations;
using Newtonsoft.Json;
using Report;
using SMS.EPA.Model.JobDescription;

public class EPAModel : IExcelRecord
{
    public float DisciplineScore { get; set; }

    public EPAEmployeeInfo Employee { get; set; } = new();

    public int EPAId { get; set; }

    public int EPAParentId { get; set; }

    public string FinalScore { get; set; }

    public string FinalScoreToExport { get; set; }

    public string Grade { get; set; }

    public bool HasDisciplineScore { get; set; }

    public bool HasMaxScoreOnKPI { get; set; }

    public bool HasMonthlyEPA { get; set; }

    public bool IsAllowedToEdit
    {
        get
        {
            bool isAllowedToEdit = !(Process.CurrentProcess == null
                                     || Process.CurrentProcess.IsEndProcess
                                     || IsProcessValid()
                                     || StatusId == (byte)EpaStatus.Closed);

            return isAllowedToEdit;
        }
    }

    public bool IsClosed => StatusId == (byte)EpaStatus.Closed;

    public bool IsEndProcessBySelf { get; set; }

    public bool IsMonthly => Period.TargetTypeId == (byte)TargetType.Monthly;

    public bool IsProbation => Period.TargetTypeId == (byte)TargetType.Probation;

    public bool IsPromotion => Period.TargetTypeId == (byte)TargetType.Promotion;

    public bool IsSemiAnnual => Period.TargetTypeId == (byte)TargetType.SemiAnnual;

    public bool IsSemiHasMonthly => HasMonthlyEPA && Period.TargetTypeId == (byte)TargetType.SemiAnnual;

    public IList<EPAKpiGroupModel> KPIGroups { get; set; } = new List<EPAKpiGroupModel>();

    public float LastDisciplineScore { get; set; }

    public int LastEPAId { get; set; }

    public string LastFinalScore { get; set; }

    public string LastGrade { get; set; }

    public float? LastPerformanceScore
    {
        get
        {
            if (LastProcesses.Count > 0)
            {
                return EvaluationProcessModel.CalculateAverageScore(LastProcesses);
            }

            return null;
        }
    }

    [JsonIgnore]
    public IList<EPAProcessModel> LastProcesses { get; set; } = new List<EPAProcessModel>();

    public float MaxDisciplineScore { get; set; }

    public int MaxFinalScore { get; } = Constants.MaxFinalScore;

    public MonthlyEPAModel MonthlyEPA { get; set; } = new();

    public string OldFinalScore { get; set; }

    public EPAPeriodModel Period { get; set; } = new();

    public EvaluationProcessModel Process { get; set; } = new();

    [JsonIgnore]
    public IList<EPAProcessModel> AllProcesses { get; set; } = new List<EPAProcessModel>();

    public RatingTypeModel RatingType { get; set; } = new();

    public IEnumerable<EPARecommendation> Recommendations { get; set; } = new List<EPARecommendation>();
    public IEnumerable<JobDescriptionGroupItem> JobDescriptions { get; set; } = new List<JobDescriptionGroupItem>();
    public EPAProbationExtendedQuestionModel ExtendedQuestion { get; set; }

    public bool RequiredCommentOnMaxScore { get; set; }
    public bool RequiredCommentOnMaxScoreForEvaluator { get; set; }

    public int Rowspan
    {
        get
        {
            List<int> numbers = new List<int>
            {
                Process.Reviewers.Count,
                Process.Managements.Count
            };

            if (!IsSemiHasMonthly)
            {
                numbers.Add(Process.Evaluators.Count);
            }

            return numbers.Max();
        }
    }

    public ScoreRatingTypeModel ScoreRatingType { get; set; }

    public byte StatusId { get; set; }

    public float SystemDisciplineScore { get; set; }

    public object ConvertToExcelRecord()
    {
        var record = new
        {
            Count = 0,
            Employee.Code,
            Nick = Employee.NickName,
            Employee.FullName,
            Department = Employee.DepartmentName,
            Section = Employee.Section,
            Team = Employee.GroupName,
            JobTitle = Employee.JobTitleName,
            OldFinalScore,
            NewFinalScore = FinalScore
        };

        return record;
    }

    private bool IsProcessValid()
    {
        return IsSemiHasMonthly
               && (Process.CurrentProcess.ProcessId == (byte)ProcessType.Self ||
                   Process.CurrentProcess.ProcessId == (byte)ProcessType.Evaluator);
    }
}