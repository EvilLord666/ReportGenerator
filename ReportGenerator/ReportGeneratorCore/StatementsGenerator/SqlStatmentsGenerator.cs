using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReportGenerator.Core.Data.Parameters;

namespace ReportGenerator.Core.StatementsGenerator
{
    public static class SqlStatmentsGenerator
    {
        public static string CreateSelectStatement(string columns, string name, ViewParameters parameters)
        {
            StringBuilder builder = new StringBuilder();
            string selectedColumns = string.IsNullOrEmpty(columns) ? SelectAllColumns : columns;
            builder.Append(String.Format(SelectTemplate, selectedColumns, name));
            if (parameters.WhereParameters != null && parameters.WhereParameters.Any())
            {
                builder.Append(WhereStatement);
                foreach (DbQueryParameter parameter in parameters.WhereParameters)
                    AppendParameter(builder, parameter);
                // todo Append Order By
                // todo Append Group By
                
            }
            return builder.ToString();
        }

        private static void AppendParameter(StringBuilder builder, DbQueryParameter parameter)
        {
            if (parameter.Conditions != null)
            {
                for (int i = 0; i < parameter.Conditions.Count; i++)
                {
                    switch (parameter.Conditions[i])
                    {
                        case JoinCondition.Or:
                        case JoinCondition.And:
                        case JoinCondition.Not:
                            builder.Append(_conditionsStatements[parameter.Conditions[i]]);
                            if (i == parameter.Conditions.Count - 1)
                                builder.Append(string.Format("{0} {1} {2}", parameter.ParameterName, parameter.ComparisonOperator, parameter.ParameterValue));
                            break;
                        case JoinCondition.In:
                        case JoinCondition.Between:
                            builder.Append(String.Format(_conditionsStatements[parameter.Conditions[i]], parameter.ParameterName, parameter.ParameterValue));
                            break;
                    }
                }
            }
        }

        public const string SelectAllColumns = "*";
        private const string SelectTemplate = "SELECT {0} FROM {1} ";
        private const string WhereStatement = " WHERE ";
        private const string OrderByTemplate = " ORDER BY {0} ";
        private const string GroupByTemplate =  "GROUP BY {0} ";

        private static IDictionary<JoinCondition, string> _conditionsStatements = new Dictionary<JoinCondition, string>()
        {
            {JoinCondition.In, "{0} IN ({1})"},
            {JoinCondition.Between, " {0} BETWEEN {1}"},
            {JoinCondition.Or, " OR "},
            {JoinCondition.And, " AND "},
            {JoinCondition.Not, " NOT "}
        };
    }
}
