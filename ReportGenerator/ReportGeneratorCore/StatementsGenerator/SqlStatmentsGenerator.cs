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
                if (parameters.GroupByParameters != null && parameters.GroupByParameters.Count > 0)
                {
                    builder.Append(string.Format(GroupByTemplate, parameters.GroupByParameters[0].ParameterName));
                    for (int i = 1; i < parameters.GroupByParameters.Count; i++)
                        builder.Append(", " + parameters.GroupByParameters[i].ParameterName);
                }

                if (parameters.OrderByParameters != null && parameters.OrderByParameters.Count > 0)
                {
                    builder.Append(string.Format(OrderByTemplate, parameters.OrderByParameters[0].ParameterName, parameters.OrderByParameters[0].ParameterValue));
                    for (int i = 1; i < parameters.OrderByParameters.Count; i++)
                        builder.Append(string.Format(", {0} {1}", parameters.OrderByParameters[i].ParameterName, parameters.OrderByParameters[i].ParameterValue));
                }
            }
            return builder.ToString();
        }

        private static void AppendParameter(StringBuilder builder, DbQueryParameter parameter)
        {
            if (parameter.Conditions != null && parameter.Conditions.Count > 0)
            {
                for (int i = 0; i < parameter.Conditions.Count; i++)
                {
                    builder.Append(_conditionsStatements[parameter.Conditions[i]]);
                    if (i == parameter.Conditions.Count - 1)
                        builder.Append(string.Format("{0} {1} {2}", parameter.ParameterName, parameter.ComparisonOperator, parameter.ParameterValue));
                }
            }
            else
            {
                builder.Append(string.Format("{0} {1} {2}", parameter.ParameterName, parameter.ComparisonOperator, parameter.ParameterValue));
            }
        }

        public const string SelectAllColumns = "*";
        private const string SelectTemplate = "SELECT {0} FROM {1} ";
        private const string WhereStatement = " WHERE ";
        private const string OrderByTemplate = " ORDER BY {0} {1}";
        private const string GroupByTemplate = " GROUP BY {0}";

        private static IDictionary<JoinCondition, string> _conditionsStatements = new Dictionary<JoinCondition, string>()
        {
            {JoinCondition.Or, " OR "},
            {JoinCondition.And, " AND "},
            {JoinCondition.Not, " NOT "}
        };
    }
}
