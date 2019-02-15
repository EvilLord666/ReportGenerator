using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using ReportGenerator.Core.Config;
using ReportGenerator.Core.Data.Parameters;
using Xunit;

namespace ReportGenerator.Core.Tests.Config
{
    public class TestExecutionConfigManager
    {
        [Theory]
        [InlineData(ReportDataSource.View)]
        [InlineData(ReportDataSource.StoredProcedure)]
        public void TestWriteConfig(ReportDataSource source)
        {
            string file = string.Format("testReport4_{0}.xml", source==ReportDataSource.View? "View":"StoredProcedure");
            ExecutionConfigManager.Write(file, GetConfig(source));
            Assert.True(File.Exists(file));
            File.Delete(file);
        }

        [Theory]
        [InlineData(ReportDataSource.StoredProcedure, GlobalTestsParams.SqlServerStoredProcedureExampleDataExecutionConfig)]
        [InlineData(ReportDataSource.View, GlobalTestsParams.SqlServerViewAdvancedDataExecutionConfig)]
        public void TestReadConfig(ReportDataSource source, string file)
        {
            ExecutionConfig expectedConfig = GetConfig(source);
            ExecutionConfig actualConfig = ExecutionConfigManager.Read(file);
            Assert.NotNull(actualConfig);
            CheckConfigs(expectedConfig, actualConfig);
        }

        private ExecutionConfig GetConfig(ReportDataSource source)
        {
            ExecutionConfig config = new ExecutionConfig();
            config.DataSource = source;
            if (source == ReportDataSource.StoredProcedure)
            {
                List<StoredProcedureParameter> procedureParameters = new List<StoredProcedureParameter>();
                procedureParameters.Add(new StoredProcedureParameter((int)SqlDbType.NVarChar, "City", "N'Yekaterinburg"));
                procedureParameters.Add(new StoredProcedureParameter((int)SqlDbType.Int, "PostalCode", "620000"));
                procedureParameters.Add(new StoredProcedureParameter((int)SqlDbType.DateTime, "DateOfBirth", "'2018-01-01'"));
                config.StoredProcedureParameters = procedureParameters;
                config.Name = "GetSitizensByCityAndDateOfBirth";
            }
            else
            {
                ViewParameters viewParameters = new ViewParameters();
                config.ViewParameters = viewParameters;
                config.Name = "CitizensView";
                List<DbQueryParameter> whereParameters = new List<DbQueryParameter>();
                whereParameters.Add(new DbQueryParameter(null, "FirstName", "=", "N'Michael'"));
                whereParameters.Add(new DbQueryParameter(new List<JoinCondition>(){ JoinCondition.And, JoinCondition.Not }, "City", "=", "N'Yekaterinburg'"));
                whereParameters.Add(new DbQueryParameter(new List<JoinCondition>() { JoinCondition.And }, "Age", "BETWEEN", "18 AND 60"));
                whereParameters.Add(new DbQueryParameter(new List<JoinCondition>() { JoinCondition.And }, "District", "IN", "(N'D1', N'A3', N'A5', N'C7')"));
                whereParameters.Add(new DbQueryParameter(new List<JoinCondition>() { JoinCondition.Or }, "Region", "!=", "N'Sverdlovskaya oblast'"));
                config.ViewParameters.WhereParameters = whereParameters;

                List<DbQueryParameter> orderByParameters = new List<DbQueryParameter>();
                orderByParameters.Add(new DbQueryParameter(null, "FirstName", null, "ASC"));
                orderByParameters.Add(new DbQueryParameter(null, "LastName", null, "DESC"));
                config.ViewParameters.OrderByParameters = orderByParameters;

                List<DbQueryParameter> groupByParameters = new List<DbQueryParameter>();
                groupByParameters.Add(new DbQueryParameter(null, "District", null, null));
                config.ViewParameters.GroupByParameters = groupByParameters;
            }
            return config;
        }

        private void CheckConfigs(ExecutionConfig expectedConfig, ExecutionConfig actualConfig)
        {
            Assert.Equal(expectedConfig.Name, actualConfig.Name);
            Assert.Equal(expectedConfig.DataSource, actualConfig.DataSource);
            if (expectedConfig.DataSource == ReportDataSource.StoredProcedure)
            {
                for (int i = 0; i < expectedConfig.StoredProcedureParameters.Count; i++)
                {
                    Assert.Equal(expectedConfig.StoredProcedureParameters[i].ParameterName, actualConfig.StoredProcedureParameters[i].ParameterName);
                    Assert.Equal(expectedConfig.StoredProcedureParameters[i].ParameterType, actualConfig.StoredProcedureParameters[i].ParameterType);
                    Assert.Equal(expectedConfig.StoredProcedureParameters[i].ParameterValue, actualConfig.StoredProcedureParameters[i].ParameterValue);
                }
            }
            else
            {
                for (int i = 0; i < expectedConfig.ViewParameters.WhereParameters.Count; i++)
                {
                    if (expectedConfig.ViewParameters.WhereParameters[i].Conditions != null)
                    {
                        for(int j=0; j < expectedConfig.ViewParameters.WhereParameters[i].Conditions.Count; j++)
                            Assert.Equal(expectedConfig.ViewParameters.WhereParameters[i].Conditions[j], actualConfig.ViewParameters.WhereParameters[i].Conditions[j]);
                    }
                    Assert.Equal(expectedConfig.ViewParameters.WhereParameters[i].ParameterName, actualConfig.ViewParameters.WhereParameters[i].ParameterName);
                    Assert.Equal(expectedConfig.ViewParameters.WhereParameters[i].ParameterValue, actualConfig.ViewParameters.WhereParameters[i].ParameterValue);
                    if(expectedConfig.ViewParameters.WhereParameters[i].ComparisonOperator != null)
                        Assert.Equal(expectedConfig.ViewParameters.WhereParameters[i].ComparisonOperator, actualConfig.ViewParameters.WhereParameters[i].ComparisonOperator);
                }

                for (int i = 0; i < expectedConfig.ViewParameters.OrderByParameters.Count; i++)
                {
                    Assert.Null(expectedConfig.ViewParameters.OrderByParameters[i].Conditions);
                    Assert.Empty(actualConfig.ViewParameters.OrderByParameters[i].Conditions);
                    Assert.Equal(expectedConfig.ViewParameters.OrderByParameters[i].ParameterName, actualConfig.ViewParameters.OrderByParameters[i].ParameterName);
                    Assert.Equal(expectedConfig.ViewParameters.OrderByParameters[i].ParameterValue, actualConfig.ViewParameters.OrderByParameters[i].ParameterValue);
                    Assert.Null(expectedConfig.ViewParameters.OrderByParameters[i].ComparisonOperator);
                    Assert.Null(actualConfig.ViewParameters.OrderByParameters[i].ComparisonOperator);
                }

                for (int i = 0; i < expectedConfig.ViewParameters.GroupByParameters.Count; i++)
                {
                    Assert.Null(expectedConfig.ViewParameters.GroupByParameters[i].Conditions);
                    Assert.Empty(actualConfig.ViewParameters.GroupByParameters[i].Conditions);
                    Assert.Equal(expectedConfig.ViewParameters.GroupByParameters[i].ParameterName, actualConfig.ViewParameters.GroupByParameters[i].ParameterName);
                    Assert.Null(expectedConfig.ViewParameters.GroupByParameters[i].ParameterValue);
                    Assert.Null(actualConfig.ViewParameters.GroupByParameters[i].ParameterValue);
                    Assert.Null(expectedConfig.ViewParameters.GroupByParameters[i].ComparisonOperator);
                    Assert.Null(actualConfig.ViewParameters.GroupByParameters[i].ComparisonOperator);
                }
            }
        }
    }
}
