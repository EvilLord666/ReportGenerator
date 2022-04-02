using System;
using System.Collections.Generic;
using System.Linq;
using ReportGenerator.Core.Data;

namespace ReportGenerator.Core.Tests.TestUtils
{
    internal static class TestData
    {
        public static DbData GetSampleData()
        {
            DbData data = new DbData();
            data.Rows.Add(GetDataRow("Иван", "Иванов", 20, "м.", "Екатеринбург", "Свердловская область"));
            data.Rows.Add(GetDataRow("Алексей", "Козлов", 25, "м.", "Нижний Тагил", "Свердловская область"));
            data.Rows.Add(GetDataRow("Татьяно", "Трололоева", 29, "ж.", "Челябинск", "Челябинская область"));
            data.Rows.Add(GetDataRow("Юра", "Первоуральский", 32, "м.", "Курган", "Курганская область"));
            data.Rows.Add(GetDataRow("Елена", "Головач", 22, "ж.", "Пермь", "Пермская область"));

            return data;
        }

        public static IList<string> GetCsvSampleData(IList<string> headers, string separator)
        {
            List<string> csvLines = new List<string>();
            csvLines.AddRange(headers);
            IList<string> lines = GetSampleData().Rows.Select(r => $"{r[0].Value}{separator}{r[1].Value}{separator}{r[2].Value}{separator}" +
                                                                                $"{r[3].Value}{separator}{r[4].Value}{separator}{r[5].Value}")
                                                 .ToList();
            csvLines.AddRange(lines);
            return csvLines;
        }

        private static IList<DbValue> GetDataRow(string firstName, string lastName, int age, string sex, string city, string region)
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
    }
}