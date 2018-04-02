using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data
{
    public class DbData
    {
        public DbData()
        {
        }

        public DbData(IList<IList<DbValue>> rows)
        {
            Rows = rows;
        }

        public IList<IList<DbValue>> Rows { get; set; }
    }
}
