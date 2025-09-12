using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using SMS.EPA.Core.Enumerations;
using SMS.EPA.Models.Report.EPAComment;
using SMS.EPA.Services.Interfaces.ExportFile;

namespace SMS.EPA.Services.Implement.ExportFile;

public class ExportCommentReportService(IHttpContextAccessor httpContextAccessor = null) : IExportCommentReportService
{
    private const string FileNamePrefix = "Comment Report - ";
    private const string TitleProfix = "Comment - ";
    private const int TableContentFirstRowIndex = 4;
    private const int TotalStaffInfoField = 5;
    private const int EvaluatorColumnIndex = 5;
    private const int ReviewerColumnIndex = 7;
    private const int ManagementColumnIndex = 9;

    private IWorkbook Workbook { get; set; }
    private ISheet Sheet { get; set; }
    private string FileName { get; set; }
    private string Title { get; set; }
    private IEnumerable<EPASummaryCommentModel> SummaryComments { get; set; }

    public void Export(EPASummaryCommentReportModel data, string templatePath)
    {
        InitializeWithTemplate(templatePath);
        GetData(data);

        FillContent();

        var response = httpContextAccessor?.HttpContext?.Response;
        if (response == null) return;

        response.Clear();
        response.ContentType = "application/vnd.ms-excel";
        response.Headers["Content-Disposition"] = $"attachment; filename={FileName}.xls";

        using var stream = new MemoryStream();
        Workbook.Write(stream, false);
        response.Body.Write(stream.ToArray());
    }

    private void InitializeWithTemplate(string templatePath)
    {
        using var fireStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        Workbook = new HSSFWorkbook(fireStream, true);
        Sheet = Workbook.GetSheetAt(0);
    }

    private void GetData(EPASummaryCommentReportModel data)
    {
        SummaryComments = data.CommentSummary;
        Title = TitleProfix + data.PeriodName;
        FileName = FileNamePrefix + data.PeriodName;
    }

    private void FillContent()
    {
        var titleCell = Sheet.GetRow(0).GetCell(0);
        titleCell.SetCellValue(Title);

        var staffFields = SummaryComments.GroupBy(comment => new { comment.Code, comment.Nickname, comment.Section, comment.Team, comment.StaffComment })
            .Select(x => x.Key).ToList();
        var rowIndex = TableContentFirstRowIndex;

        foreach (var staffField in staffFields)
        {
            var colIndex = 0;
            var evaluatorRow = new Dictionary<string, int>
            {
                { "Evaluator", rowIndex },
                { "Reviewer", rowIndex },
                { "Management", rowIndex }
            };

            var curRow = Sheet.CreateRow(rowIndex);

            curRow.CreateCell(colIndex++).SetCellValue(staffField.Code);
            curRow.CreateCell(colIndex++).SetCellValue(staffField.Nickname);
            curRow.CreateCell(colIndex++).SetCellValue(staffField.Section);
            curRow.CreateCell(colIndex++).SetCellValue(staffField.Team);
            curRow.CreateCell(colIndex).SetCellValue(staffField.StaffComment);

            FillEvaluatorFields(evaluatorRow, staffField.Code);

            var nextStaffRowIndex = evaluatorRow.Values.Max();
            MergeStaffFields(rowIndex, nextStaffRowIndex - 1);

            rowIndex = nextStaffRowIndex;
        }
    }

    private void FillEvaluatorFields(Dictionary<string, int> evaluatorRow, string staffCode)
    {
        var evaluatorComments = SummaryComments.Where(comment => comment.Code == staffCode);

        foreach (var evaluatorComment in evaluatorComments)
        {
            switch (evaluatorComment.ProcessId)
            {
                case (byte)ProcessType.Evaluator:
                    var evaluatorRowIndex = evaluatorRow["Evaluator"];
                    GetExcelRow(evaluatorRowIndex).CreateCell(EvaluatorColumnIndex).SetCellValue(evaluatorComment.EvaluatorNickName);
                    GetExcelRow(evaluatorRowIndex).CreateCell(EvaluatorColumnIndex + 1).SetCellValue(evaluatorComment.EvaluatorComment);
                    evaluatorRow["Evaluator"] = evaluatorRowIndex + 1;
                    break;

                case (byte)ProcessType.Reviewer:
                    var reviewerRowIndex = evaluatorRow["Reviewer"];
                    GetExcelRow(reviewerRowIndex).CreateCell(ReviewerColumnIndex).SetCellValue(evaluatorComment.EvaluatorNickName);
                    GetExcelRow(reviewerRowIndex).CreateCell(ReviewerColumnIndex + 1).SetCellValue(evaluatorComment.EvaluatorComment);
                    evaluatorRow["Reviewer"] = reviewerRowIndex + 1;
                    break;

                case (byte)ProcessType.Management:
                    var managementRowIndex = evaluatorRow["Management"];
                    GetExcelRow(managementRowIndex).CreateCell(ManagementColumnIndex).SetCellValue(evaluatorComment.EvaluatorNickName);
                    GetExcelRow(managementRowIndex).CreateCell(ManagementColumnIndex + 1).SetCellValue(evaluatorComment.EvaluatorComment);
                    evaluatorRow["Management"] = managementRowIndex + 1;
                    break;

                default:
                    continue;
            }
        }
    }

    private void MergeStaffFields(int startRowIndex, int lasRowIndex)
    {
        for (var colIndex = 0; colIndex < TotalStaffInfoField; colIndex++)
        {
            var mergeRegion = new CellRangeAddress(startRowIndex, lasRowIndex, colIndex, colIndex);
            Sheet.AddMergedRegion(mergeRegion);
        }
    }

    private IRow GetExcelRow(int rowIndex)
    {
        return Sheet.GetRow(rowIndex) ?? Sheet.CreateRow(rowIndex);
    }
}