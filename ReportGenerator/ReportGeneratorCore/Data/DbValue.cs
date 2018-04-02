using System;
using System.Collections.Generic;
using System.Text;

namespace ReportGenerator.Core.Data
{
    public class DbValue
    {
        public DbValue()
        {
        }

        public DbValue(string column, object value)
        {
            Column = column;
            Value = value;
        }

        public string Column { get; private set; }

        public object Value { get; private set; }
    }
}
