using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.EmployeeEPA;
using SMS.EPA.Models.Report.EPADetail;
using SMS.EPA.Resources;

namespace SMS.EPA.Services.Implement.ExportFile;

public class MonthlyEpaDetailReportService : EpaDetailReportServiceBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MonthlyEpaDetailReportService(IHttpContextAccessor httpContextAccessor = null) 
        : base(new CommonService(), new EmployeeEPAService())
    {
        _httpContextAccessor = httpContextAccessor;
        SheetTittle = Lang.MonthlyEPADetailReportSheet;
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
        Lang.TargetTemplate,
        Lang.SelfScore,
        Lang.EvaluationScore,
        Lang.ReviewerScore,
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
        response.Headers["Content-Disposition"] = $"attachment; filename={Lang.MonthlyEPADetailReportName}.xls";

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

            var currentColumn = WriteCommonCells(epaInfo, row, currentRow++);

            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleLeft, OddStyleLeft);

            ICell cell = row.CreateCell(currentColumn++);
            cell.CellStyle = BodyStyle;
            cell.SetCellValue(epaInfo.Period.TargetTemplate);
            dynamic score = CalculateScore(epaInfo);

            currentColumn = WriteScoreCells(epaInfo, currentColumn, row, score);
            BodyStyle = ApplyEvenOddBodyStyle(EvenStyleLeft, OddStyleLeft);
            BodyStyle = epaInfo.StatusId == (byte)EpaStatus.Open
                ? ApplyEvenOddBodyStyle(EvenRedStyle, OddRedStyle)
                : ApplyEvenOddBodyStyle(EvenStyleCenter, OddStyleCenter);

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
        var disciplineScore = Lang.NotAvailable;
        var selfScore = Lang.NotAvailable;
        var evaluatorScore = Lang.NotAvailable;
        var reviewerScore = Lang.NotAvailable;

        if (epaInfo.EPAId > 0)
        {
            selfScore = GetSelfScore(epaInfo);
            evaluatorScore = GetEvaluatorScore(epaInfo);
            reviewerScore = GetReviewerScore(epaInfo);
        }

        var finalScore = epaInfo.FinalScoreToExport;
        var grade = GetGrade(epaInfo, finalScore);

        return new
        {
            SelfScore = selfScore,
            EvaluatorScore = evaluatorScore,
            ReviewerScore = reviewerScore,
            DisciplineScore = disciplineScore,
            FinalScore = finalScore,
            Grade = grade
        };
    }
}