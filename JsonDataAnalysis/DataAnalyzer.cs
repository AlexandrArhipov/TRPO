using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonDataAnalysis
{
    public class DataAnalyzer
    {
        private DataTable _dataTable;
        
        public DataAnalyzer(string jsonText)
        {
            JObject data = JObject.Parse(jsonText);

            JToken[] columnNames = data["table"]["columnNames"].Children().ToArray();
            JToken[] columnTypes = data["table"]["columnTypes"].Children().ToArray();
            if (columnNames.Length != columnTypes.Length)
                throw new JsonException("columnNames.Length = " + columnNames.Length +  
                                        " doesn't match columnTypes.Length = " + columnTypes.Length + ".");
            
            JToken[] rows = data["table"]["rows"].Children().ToArray();
            
            _dataTable = new DataTable();

            for (int i = 0; i < columnNames.Length; i++)
                _dataTable.Columns.Add(
                    new DataColumn(columnNames[i].ToString(), GetTypeFromJsonRepresentation(columnTypes[i].ToString())));

            foreach (var jToken in rows)
                _dataTable.Rows.Add(jToken.ToObject<object[]>());
        }

        private Type GetTypeFromJsonRepresentation(string type)
        {
            switch (type)
            {
                case "String": return typeof(string);
                case "float": return typeof(float);
                case "byte": return typeof(byte);
                default: throw new ArgumentException("Type " + type + " wasn't expected.");
            }
        }

        public Dictionary<string, object> GetStats(string columnName)
        {            
            string columnNameQc = columnName + "_qc";
            
            if (!_dataTable.Columns.Contains(columnName) || !_dataTable.Columns.Contains(columnNameQc))
                throw new ArgumentException("Column " + columnName + " isn't represented in DataTable");

            int indexOfFirstValidRecord = 0;

            foreach (DataRow row in _dataTable.Rows)
            {
                if (row.Field<byte>(columnNameQc) == 0)
                    break;

                indexOfFirstValidRecord++;
            }
            
            List<DataRow> minValueRows = new List<DataRow> { _dataTable.Rows[indexOfFirstValidRecord] };
            List<DataRow> maxValueRows = new List<DataRow> { _dataTable.Rows[indexOfFirstValidRecord] };

            float minValue = minValueRows[0].Field<float>(columnName);
            float maxValue = maxValueRows[0].Field<float>(columnName);

            float sum = 0;
            int validRowsCount = 0;

            foreach (DataRow row in _dataTable.AsEnumerable().Skip(indexOfFirstValidRecord))
            {
                if (row.Field<byte>(columnNameQc) != 0)
                    continue;
                
                float currentValue = row.Field<float>(columnName);

                if (currentValue < minValue)
                {
                    minValueRows = new List<DataRow> {row};
                    minValue = currentValue;
                }
                else if (Math.Abs(currentValue - minValue) < 0.0001f)
                    minValueRows.Add(row);

                if (currentValue > maxValue)
                {
                    maxValueRows = new List<DataRow> {row};
                    maxValue = currentValue;
                }
                else if (Math.Abs(currentValue - maxValue) < 0.0001f)
                    maxValueRows.Add(row);
                
                sum += currentValue;
                validRowsCount++;
            }

            var avgValue = sum / validRowsCount;

            Dictionary<string, object> result = new Dictionary<string, object>(8)
            {
                {"start_date", DateTime.Parse(_dataTable.Rows[0].Field<string>("time")).ToShortDateString()},
                {"end_date", DateTime.Parse(_dataTable.Rows[_dataTable.Rows.Count - 1].Field<string>("time")).ToShortDateString()},
                {"num_records", validRowsCount},
                {"min_" + columnName, minValueRows[0][columnName]},
                {"min_times", minValueRows.Select(row => row["time"])},
                {"max_" + columnName, maxValueRows[0][columnName]},
                {"max_times", maxValueRows.Select(row => row["time"])},
                {"avg_" + columnName, avgValue}
            };

            return result;
        }
    }
}