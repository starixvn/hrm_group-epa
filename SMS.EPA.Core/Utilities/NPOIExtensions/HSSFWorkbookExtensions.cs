namespace SMS.EPA.Core.Utilities.NPOIExtensions
{
    using Microsoft.AspNetCore.Http;
    using NPOI.SS.UserModel;

    public static class HSSFWorkbookExtensions
    {
        public static ISheet CloneSheet(this IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            var sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            var newSheet = workbook.CloneSheet(sheetIndex);
            var newSheetIndex = workbook.GetSheetIndex(newSheet);
            workbook.SetSheetName(newSheetIndex, newSheetName);
            return newSheet;
        }

        public static void ToResponseStream(this IWorkbook workbook, HttpResponse response, string fileName)
        {
            if (workbook != null && response != null)
            {
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}.xls");

                workbook.Write(response.Body);
            }
        }
    }
}