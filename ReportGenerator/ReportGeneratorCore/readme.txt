В конфигурации все параметры, описывающие параметры хранимых процедур задаются 3 атрибутами:
 - Тип параметра (целочисленное значения, см. SqlDbType для SQL Server
                  -> https://msdn.microsoft.com/ru-ru/library/system.data.sqldbtype(v=vs.110).aspx)
 - Имя параметра
 - Значения параметра (строковые параметры, даты, уникальные идентификаторы необходимо задавать в кавычках)

Пример конфигурации хранимой процедуры:

<?xml version="1.0"?>
<ExecutionConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <!-- 
       If use SQL Server see for parameters Type : https://docs.microsoft.com/ru-ru/dotnet/api/system.data.sqldbtype 
   -->
  <DataSource>StoredProcedure</DataSource>
  <Name>GetSitizensByCityAndDateOfBirth</Name>
  <StoredProcedureParameters>
    <!-- SQL Server NVarChar enum value is 12-->
    <ParameterType>12</ParameterType> 
    <ParameterName>City</ParameterName>
    <ParameterValue xsi:type="xsd:string">N'Yekaterinburg</ParameterValue>
  </StoredProcedureParameters>
  <StoredProcedureParameters>
    <!-- SQL Server Int enum value is 8-->
    <ParameterType>8</ParameterType>
    <ParameterName>PostalCode</ParameterName>
    <ParameterValue xsi:type="xsd:string">620000</ParameterValue>
  </StoredProcedureParameters>
  <StoredProcedureParameters>
    <!-- SQL Server Date enum value is 4-->
    <ParameterType>4</ParameterType>
    <ParameterName>DateOfBirth</ParameterName>
    <ParameterValue xsi:type="xsd:string">'2018-01-01'</ParameterValue>
  </StoredProcedureParameters>
</ExecutionConfig>

##################################################################################################################################


Для представления существуют 3 типа параметров для фильтрации строк:
 - Where параметры
 - Group By параметры
 - Order By параметры
 Все эти типы задаются одним типом данных - DbQueryParameter
 DbQueryParameter содержит 4 поля:
  - коллекция Join Condition (необходимо только для Where параметров). Join Condition способ объединения парметров логическое И (AND) или
    логическое ИЛИ (OR), но, поскольку существует возможность комбинирования парметров в SQL - AND NOT... и OR NOT, не должен указываться
	для первого Where параметра (см. пример ниже)
  - имя парметра
  - оператор сравнения (используется только для Where-параметров) - все возможные значения для SQL (>, <, =, IS, IS NOT, NOT, IN, BETWEEN, 
    LIKE, и т.п.)
  - значение:
     для Where - значение используемое в операторе сравнения
	 для OrderBy - ASC либо DESC

Пример конфигурации представления:

<?xml version="1.0"?>
<ExecutionConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <DataSource>View</DataSource>
  <Name>CitizensView</Name>
  <ViewParameters>
    <WhereParameters>
      <DbQueryParameter>
        <ParameterName>FirstName</ParameterName>
        <ParameterValue>N'Michael'</ParameterValue>
        <ComparisonOperator>=</ComparisonOperator>
      </DbQueryParameter>
      <DbQueryParameter>
        <Conditions>
          <JoinCondition>And</JoinCondition>
          <JoinCondition>Not</JoinCondition>
        </Conditions>
        <ParameterName>City</ParameterName>
        <ParameterValue>N'Yekaterinburg'</ParameterValue>
        <ComparisonOperator>=</ComparisonOperator>
      </DbQueryParameter>
      <DbQueryParameter>
        <Conditions>
          <JoinCondition>Between</JoinCondition>
        </Conditions>
        <ParameterName>Age</ParameterName>
        <ParameterValue>18 AND 60</ParameterValue>
      </DbQueryParameter>
      <DbQueryParameter>
        <Conditions>
          <JoinCondition>In</JoinCondition>
        </Conditions>
        <ParameterName>District</ParameterName>
        <ParameterValue>N'D1', N'A3', N'A5', N'C7'</ParameterValue>
      </DbQueryParameter>
      <DbQueryParameter>
        <Conditions>
          <JoinCondition>Or</JoinCondition>
        </Conditions>
        <ParameterName>Region</ParameterName>
        <ParameterValue>N'Sverdlovskaya oblast'</ParameterValue>
        <ComparisonOperator>!=</ComparisonOperator>
      </DbQueryParameter>
    </WhereParameters>
    <OrderByParameters>
      <DbQueryParameter>
        <ParameterName>FirstName</ParameterName>
        <ParameterValue>ASC</ParameterValue>
      </DbQueryParameter>
      <DbQueryParameter>
        <ParameterName>LastName</ParameterName>
        <ParameterValue>DESC</ParameterValue>
      </DbQueryParameter>
    </OrderByParameters>
    <GroupByParameters>
      <DbQueryParameter>
        <ParameterName>District</ParameterName>
      </DbQueryParameter>
    </GroupByParameters>
  </ViewParameters>
</ExecutionConfig>