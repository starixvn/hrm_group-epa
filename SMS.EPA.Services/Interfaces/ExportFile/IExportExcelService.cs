using System.Threading.Tasks;
using SMS.EPA.Models.Report;

namespace SMS.EPA.Services.Interfaces.ExportFile;

using System.Collections.Generic;
using NPOI.SS.UserModel;

public interface IExportExcelService
{
    IWorkbook Workbook { get; set; }

    ISheet Sheet { get; set; }

    int TitleRowIndex { get; set; }

    int HeaderRowIndex { get; set; }

    int ContentRowStartIndex { get; set; }

    int TotalColumns { get; }

    string FileName { get; set; }

    string SheetName { get; set; }

    string Title { get; set; }

    ICellStyle TitleStyle { get; set; }

    IList<ICellStyle> HeaderStyles { get; set; }

    IList<ICellStyle> ColumnsStyles { get; set; }

    /// <summary>
    /// Hold the condition to apply a style to a cell
    /// </summary>
    IList<ConditionalCellStyle> ConditionalCellStyles { get; set; }

    void Initialize();

    void InitializeWithTemplate(string templatePath);

    /// <summary>
    /// Return index of newly created style in ColumnsStyles list
    /// </summary>
    /// <returns></returns>
    int CreateNewColumnStyle();

    /// <summary>
    /// Export Excel file
    /// </summary>
    /// <param name="datas">data to export</param>
    /// <param name="firstColumnAutoIncrement">set to true to set first column is row number with auto increment</param>
    Task Export(IEnumerable<IExcelRecord> datas, bool firstColumnAutoIncrement = false);

    /// <summary>
    /// Export Excel file using Template
    /// </summary>
    /// <param name="datas">data to export</param>
    /// /// <param name="firstColumnAutoIncrement">set to true to set first column is row number with auto increment</param>
    /// <param name="rewriteTitle">Rewrite title with your text</param>
    Task ExportWithTemplate(IEnumerable<IExcelRecord> datas, bool firstColumnAutoIncrement = false, bool rewriteTitle = false);
}