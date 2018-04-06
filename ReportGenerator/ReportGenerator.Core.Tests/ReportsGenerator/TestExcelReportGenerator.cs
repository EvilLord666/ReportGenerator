using System.Collections.Generic;
using System.IO;
using ReportGenerator.Core.Data;
using ReportGenerator.Core.ReportsGenerator;
using Xunit;

namespace ReportGenerator.Core.Tests.ReportsGenerator
{
    public class TestExcelReportGenerator
    {
        [Fact]
        public void TestGenerate()
        {
            if (File.Exists(ReportFile))
                File.Delete(ReportFile);
            IReportGenerator generator = new ExcelReportGenerator(TestExcelTemplate, ReportFile);
            // Worksheet - 1, start row - 2, start column - 3
            object[] parameters = {1, 2, 3};
            DbData data = new DbData();
            data.Rows.Add(GetDataRow("Иван", "Иванов", 20, "м.", "Екатеринбург", "Свердловская область"));
            data.Rows.Add(GetDataRow("Алексей", "Козлов", 25, "м.", "Нижний Тагил", "Свердловская область"));
            data.Rows.Add(GetDataRow("Татьяно", "Трололоева", 29, "ж.", "Челябинск", "Челябинская область"));
            data.Rows.Add(GetDataRow("Юра", "Первоуральский", 32, "м.", "Курган", "Курганская область"));
            data.Rows.Add(GetDataRow("Елена", "Головач", 22, "ж.", "Пермь", "Пермская область"));

            bool result = generator.Generate(data, parameters);
            Assert.True(result);
            Assert.True(File.Exists(ReportFile));

            // todo: umv: add read excel doc and check

            if (File.Exists(ReportFile))
                File.Delete(ReportFile);
        }

        private IList<DbValue> GetDataRow(string firstName, string lastName, int age, string sex, string city, string region)
        {
            return (new[]
            {
                new DbValue("FirstName", firstName),
                new DbValue("LastName", lastName),
                new DbValue("Age", age),
                new DbValue("Sex", sex),
                new DbValue("City", city),
                new DbValue("Region", region),
            });
        }

        private const string TestExcelTemplate = @"..\..\..\TestExcelTemplates\CitizensTemplate.xlsx";
        private const string ReportFile = @".\Report.xlsx";
    }
}
