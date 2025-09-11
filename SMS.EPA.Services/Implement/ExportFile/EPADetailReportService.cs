using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using SMS.EPA.Core.Constants;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.Report.EPADetail;
using SMS.EPA.Resources;

namespace SMS.EPA.Services.Implement.ExportFile;

public class EpaDetailReportService : EpaDetailReportServiceBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EpaDetailReportService(IHttpContextAccessor httpContextAccessor = null) 
        : base(new CommonService(), new EmployeeEPAService())
    {
        _httpContextAccessor = httpContextAccessor;
        SheetTittle = Lang.EPADetailReportSheet;
    }

    public override IList<string> GetColumnsName => new List<string>
    {
        Lang.SymbolCount,
        Lang.Code,
        Lang.NickName,
        Lang.FullName,
        Lang.Category,
        Lang.Department,
        Lang.Section,
        Lang.Team,
        Lang.JobTitle,
        Lang.GroupDate,
        Lang.TargetType,
        Lang.TargetTemplate,
        Lang.SelfScore,
        Lang.EvaluationScore,
        Lang.ReviewerScore,
        Lang.ManagementScore,
        Lang.DisciplineScore,
        Lang.FinalScore,
        Lang.Grade
    };

    public override void Render(EPADetailModel epaDetail)
    {
        if (Workbook == null)
        {
            Export(epaDetail);
        }

        var response = _httpContextAccessor?.HttpContext?.Response;
        if (response == null) return;

        response.Clear();
        response.ContentType = "application/vnd.ms-excel";

        var contentDisposition = string.IsNullOrEmpty(epaDetail.EpaYear)
                                 || string.IsNullOrEmpty(epaDetail.EpaEvaluationPeriod)
            ? $"attachment; filename={Lang.EPADetailReportName}.xls"
            : $"attachment; filename={epaDetail.EpaYear}_{epaDetail.EpaEvaluationPeriod}_{Lang.EPADetailReportName}.xls";

        response.Headers["Content-Disposition"] = contentDisposition;

        using var stream = new MemoryStream();
        Workbook.Write(stream);
        response.Body.Write(stream.ToArray());
    }

    public override void WriteContentTable()
    {
        CreateDefaultStyles();
        int currentRow = 1;

        foreach (EPAModel epaInfo in EpaDetail.EpaInfos)
        {
            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleCenter, OddStyleCenter);

            IRow row = Sheet.CreateRow(CurrentRow);

            int currentColumn = WriteCommonCells(epaInfo, row, currentRow++);

            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleLeft, OddStyleLeft);

            ICell cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(epaInfo.Period.TargetTypeName);

            cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(epaInfo.Period.TargetTemplate);

            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleCenter, OddStyleCenter);

            dynamic score = CalculateScore(epaInfo);

            currentColumn = WriteScoreCells(epaInfo, currentColumn, row, score);

            BodyStyle = epaInfo.Process.Managements.Any() && epaInfo.Process.Managements[0].StatusId ==
                (int)ProcessStatusType.InProgress
                    ? ApplyEvenOddBodyStyle(EvenRedStyle, OddRedStyle)
                    : ApplyEvenOddBodyStyle(EvenStyleCenter, OddStyleCenter);

            cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(GetScoreByPhaseSelected(EpaDetail.PhaseSelecteds, score.ManagementScore.ToString(), (byte)ProcessType.Management));

            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleCenter, OddStyleCenter);

            cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(score.DisciplineScore);

            if (epaInfo.StatusId == (byte)EpaStatus.Open)
            {
                BodyStyle = ApplyEvenOddBodyStyle(EvenRedStyle, OddRedStyle);
            }

            cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(score.FinalScore);

            cell = row.CreateCell(currentColumn);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(score.Grade);

            CurrentRow++;
        }
    }

    protected override dynamic CalculateScore(EPAModel epaInfo)
    {
        var selfScore = Lang.NotAvailable;
        var evaluatorScore = Lang.NotAvailable;
        var reviewerScore = Lang.NotAvailable;
        var managementScore = Lang.NotAvailable;

        var disciplineScore = Math.Abs(epaInfo.DisciplineScore) > Constants.Tolerance ? epaInfo.DisciplineScore.ToString(CultureInfo.InvariantCulture) : "-";

        if (epaInfo.HasDisciplineScore)
        {
            disciplineScore = epaInfo.DisciplineScore < 0 ? "0" : epaInfo.DisciplineScore.ToString(CultureInfo.InvariantCulture);
        }

        if (epaInfo.EPAId > 0)
        {
            selfScore = GetSelfScore(epaInfo);
            evaluatorScore = GetEvaluatorScore(epaInfo);
            reviewerScore = GetReviewerScore(epaInfo);
            managementScore = GetManagementScore(epaInfo);

            if (epaInfo.IsSemiHasMonthly)
            {
                selfScore = epaInfo.MonthlyEPA.AverageSelfScoreConverted?.ToString() ?? Lang.NotAvailable;

                evaluatorScore = epaInfo.MonthlyEPA.AverageEvaluatorScoreConverted?.ToString() ?? Lang.NotAvailable;
            }
        }

        var finalScore = epaInfo.FinalScoreToExport;
        var grade = GetGrade(epaInfo, finalScore);

        return new
        {
            SelfScore = selfScore,
            EvaluatorScore = evaluatorScore,
            ReviewerScore = reviewerScore,
            ManagementScore = managementScore,
            DisciplineScore = disciplineScore,
            FinalScore = finalScore,
            Grade = grade
        };
    }

    private static string GetManagementScore(EPAModel epaInfo)
    {
        string managementScore = Lang.NotAvailable;

        if (epaInfo.Process.Managements.Any())
        {
            if (epaInfo.Process.Managements[0].StatusId == (byte)ProcessStatusType.InProgress)
            {
                managementScore = epaInfo.Process.AverageScoreByManagements?.ToString() ?? Lang.NotAvailable;
            }

            if (epaInfo.Process.Managements[0].StatusId == (byte)ProcessStatusType.Closed)
            {
                managementScore = epaInfo.Process.AverageScoreByManagements?.ToString() ?? Lang.NotAvailable;
            }
        }

        return managementScore;
    }
}