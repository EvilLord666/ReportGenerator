using System;
using System.Collections.Generic;
using System.Linq;
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

        public ExecutionConfig(ReportDataSource dataSource, string name, IList<StoredProcedureParameter> storedProcedureParameters,
                               ViewParameters viewParameters)
        {
            DataSource = dataSource;
            Name = name;
            StoredProcedureParameters = storedProcedureParameters.ToList();
            ViewParameters = viewParameters;
        }

        [XmlElement("DataSource")]
        public ReportDataSource DataSource { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("StoredProcedureParameters")]
        public List<StoredProcedureParameter> StoredProcedureParameters { get; set; }

        [XmlElement("ViewParameters")]
        public ViewParameters ViewParameters { get; set; }
    }
}
