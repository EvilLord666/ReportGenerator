using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.Config
{
    public enum ReportDataSource
    {
        View,
        StoredProcedure
    }

    [XmlRoot("ExecutionConfig")]
    public class ExecutionConfig
    {
        public ExecutionConfig()
        {
        }

        public ExecutionConfig(ReportDataSource dataSource, IList<StoredProcedureParameter> storedProcedureParameters,
                               ViewParameters viewParameters)
        {
            DataSource = dataSource;
            StoredProcedureParameters = storedProcedureParameters;
            ViewParameters = viewParameters;
        }


        [XmlElement("DataSource")]
        public ReportDataSource DataSource { get; set; }

        [XmlElement("StoredProcedureParameters")]
        public IList<StoredProcedureParameter> StoredProcedureParameters { get; set; }

        [XmlElement("ViewParameters")]
        public ViewParameters ViewParameters { get; set; }
    }
}
