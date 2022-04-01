using System.Collections.Generic;
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