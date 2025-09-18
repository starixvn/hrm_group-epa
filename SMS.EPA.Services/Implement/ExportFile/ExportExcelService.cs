using System.Threading.Tasks;
using SMS.EPA.Core.Utilities.NPOIExtensions;
using SMS.EPA.Models.Report;

namespace SMS.EPA.Services.Implement.ExportFile;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Interfaces.ExportFile;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class ExportExcelService(IHttpContextAccessor httpContextAccessor = null) : IExportExcelService
{
    public ExportExcelService(string fileName, string sheetName, string title, IHttpContextAccessor httpContextAccessor = null) : this(httpContextAccessor)
    {
        FileName = fileName;
        SheetName = sheetName;
        Title = title;
    }

    const int defaultContentRowStart = 2;

    public IWorkbook Workbook { get; set; }

    public ISheet Sheet { get; set; }

    public int TitleRowIndex { get; set; }

    public int HeaderRowIndex { get; set; } = 1;

    public int ContentRowStartIndex { get; set; } = defaultContentRowStart;

    public int TotalColumns { get; private set; }

    public string FileName { get; set; }

    public string SheetName { get; set; }

    public string Title { get; set; }

    public ICellStyle TitleStyle { get; set; }

    public IList<ICellStyle> HeaderStyles { get; set; }

    public IList<ICellStyle> ColumnsStyles { get; set; }

    public IList<ConditionalCellStyle> ConditionalCellStyles { get; set; }

    private void SetResponseHeaders(HttpResponse response, string fileName)
    {
        // Determine file format based on workbook type
        bool isXlsx = Workbook is XSSFWorkbook;
        string contentType = isXlsx ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/vnd.ms-excel";
        string fileExtension = isXlsx ? "xlsx" : "xls";
        
        response.ContentType = contentType;
        response.Headers["Content-Disposition"] = $"attachment; filename={fileName}.{fileExtension}";
    }

    public void Initialize()
    {
        // Default to HSSFWorkbook for backward compatibility
        Workbook = new HSSFWorkbook();
        Sheet = Workbook.CreateSheet(SheetName);

        TitleStyle = Workbook.CreateCellStyle();
        HeaderStyles = new List<ICellStyle>();

        ConditionalCellStyles = new List<ConditionalCellStyle>();
        ColumnsStyles = new List<ICellStyle>();
    }

    public void InitializeWithTemplate(string templatePath)
    {
        using var fireStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
        
        // Auto-detect file format based on extension
        if (templatePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            Workbook = new XSSFWorkbook(fireStream);
        }
        else
        {
            Workbook = new HSSFWorkbook(fireStream, true);
        }
        
        Sheet = Workbook.GetSheetAt(0);

        ConditionalCellStyles = new List<ConditionalCellStyle>();
        ColumnsStyles = new List<ICellStyle>();
    }

    private void WriteTitle()
    {
        var titleRow = Sheet.CreateRow(TitleRowIndex);
        var cell = titleRow.CreateCell(0, CellType.String);
        cell.SetCellValue(Title);
        cell.CellStyle = TitleStyle;
    }

    private void WriteHeaders(object sampleRecord)
    {
        var recordType = sampleRecord.GetType();

        var columns = recordType.GetProperties();

        var headerRow = Sheet.CreateRow(HeaderRowIndex);

        for (var index = 0; index < TotalColumns; index++)
        {
            var propertyInfo = columns[index];
            headerRow.CreateCell(index).SetCellValue(propertyInfo.Name);
            HeaderStyles.Add(Workbook.CreateCellStyle());
        }
    }

    private void WriteData(IEnumerable<IExcelRecord> datas, bool firstColumnAutoIncrement)
    {
        var rowId = ContentRowStartIndex;

        var rowNumber = 1;

        foreach (var data in datas)
        {
            var row = Sheet.CreateRow(rowId);
            WriteEachColumn(firstColumnAutoIncrement, rowNumber, data, row);

            rowId++;
            rowNumber++;
        }
    }

    private void WriteEachColumn(bool firstColumnAutoIncrement, int rowNumber, IExcelRecord data, IRow row)
    {
        var objectData = data.ConvertToExcelRecord();
        var infos = objectData.GetType().GetProperties();

        for (var index = 0; index < TotalColumns; index++)
        {
            var cell = row.CreateCell(index);

            var value = infos[index].GetValue(objectData, null);

            cell.SetCellValue(value);

            if (index == 0 && firstColumnAutoIncrement)
            {
                cell.SetCellValue(rowNumber);
            }

            foreach (var conditionalCellStyle in ConditionalCellStyles)
            {
                if (index == conditionalCellStyle.ColumnIndex
                    && conditionalCellStyle.Condition(objectData))
                {
                    cell.CellStyle = ColumnsStyles[conditionalCellStyle.StyleIndex];
                }
            }
        }
    }

    public int CreateNewColumnStyle()
    {
        if (ColumnsStyles != null)
        {
            var style = Workbook.CreateCellStyle();
            ColumnsStyles.Add(style);
            return ColumnsStyles.IndexOf(style);
        }

        return 0;
    }

    private void FormatSheet()
    {
        // Only auto-size columns for HSSFWorkbook (Excel 97-2003)
        if (Workbook is HSSFWorkbook)
        {
            for (var i = 0; i < TotalColumns; i++)
            {
                Sheet.AutoSizeColumn(i);
            }
        }

        Sheet.CreateFreezePane(0, ContentRowStartIndex);
    }

    private void CopyTitleStyleFromTemplate()
    {
        TitleStyle = Sheet.GetRow(TitleRowIndex).GetCell(0).CellStyle;
    }

    public async Task Export(IEnumerable<IExcelRecord> datas, bool firstColumnAutoIncrement = false)
    {
        var dataList = datas as IList<IExcelRecord> ?? datas.ToList();

        if (!dataList.Any())
        {
            return;
        }

        TotalColumns = dataList[0].ConvertToExcelRecord().GetType().GetProperties().Length;

        WriteTitle();

        WriteHeaders(dataList[0].ConvertToExcelRecord());

        WriteData(dataList, firstColumnAutoIncrement);

        FormatSheet();

        var response = httpContextAccessor?.HttpContext?.Response;
        if (response == null)
        {
            return;
        }

        response.Clear();
        SetResponseHeaders(response, FileName);

        using var stream = new MemoryStream();
        const bool leaveOpen = false;
        Workbook.Write(stream, leaveOpen);
        await response.Body.WriteAsync(stream.ToArray());
    }

    public async Task ExportWithTemplate(
        IEnumerable<IExcelRecord> datas,
        bool firstColumnAutoIncrement = false,
        bool rewriteTitle = false)
    {
        var dataList = datas as IList<IExcelRecord> ?? datas.ToList();

        if (!dataList.Any())
        {
            return;
        }

        TotalColumns = dataList[0].ConvertToExcelRecord().GetType().GetProperties().Length;

        if (rewriteTitle)
        {
            CopyTitleStyleFromTemplate();
            WriteTitle();
        }

        WriteData(dataList, firstColumnAutoIncrement);

        FormatSheet();

        var response = httpContextAccessor?.HttpContext?.Response;
        if (response == null)
        {
            return;
        }

        response.Clear();
        SetResponseHeaders(response, FileName);

        using var stream = new MemoryStream();
        const bool leaveOpen = false;
        Workbook.Write(stream, leaveOpen);
        await response.Body.WriteAsync(stream.ToArray());
    }
}