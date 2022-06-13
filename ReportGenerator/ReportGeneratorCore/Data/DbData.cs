using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data
{
    public class DbData
    {
        public DbData()
        {
            Rows = new List<IList<DbValue>>();
        }

        public DbData(List<IList<DbValue>> rows)
        {
            Rows = rows;
        }

        public List<IList<DbValue>> Rows { get; set; }
    }
}
