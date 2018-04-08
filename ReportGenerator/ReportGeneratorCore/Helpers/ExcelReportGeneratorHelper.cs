using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Helpers
{
    public static class ExcelReportGeneratorHelper
    {
        public static object[] CreateParameters(int workSheetNumber, int row, int column)
        {
            return new object[] {workSheetNumber, row, column} ;
        }
    }
}
