using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAppTestingFramework.Helpers
{
    public static class SqlExtensionMethods
    {

        public static void ExecuteNonQuery(this SqlConnection connection, string query)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            using (var command = CreateCommand(connection, query))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void ExecuteReader(this SqlConnection connection, string query, Action<SqlDataReader> action)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            using (var command = CreateCommand(connection, query))
            using (var reader = command.ExecuteReader())
            {
                action(reader);
            }
        }

        public static void InsertRecord(this SqlConnection connection, string tableName,
            Dictionary<string, object> data)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

            if (data == null || data.Count == 0)
                throw new ArgumentException("Data dictionary cannot be null or empty.", nameof(data));

            using (var command = CreateInsertCommand(connection, tableName, data))
            {
                command.ExecuteNonQuery();
            }
        }

        public static void VerifyCellData(this SqlConnection connection, string tableName,
            string targetColumnName, string conditionColumnName, object conditionValue,
            object expectedData, bool convertData = false)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            string selectQuery = $"SELECT TOP 1 {targetColumnName} FROM {tableName} WHERE {conditionColumnName} = @ConditionValue";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@ConditionValue", conditionValue);

                object actualData = command.ExecuteScalar();

                if (convertData)
                {
                    switch (expectedData)
                    {
                        case int expectedInt:
                            int actualInt = Convert.ToInt32(actualData);
                            Assert.AreEqual(expectedInt, actualInt);
                            break;
                        case string expectedString:
                            string actualString = Convert.ToString(actualData);
                            Assert.AreEqual(expectedString, actualString);
                            break;
                        case float expectedFloat:
                            float actualFloat = Convert.ToSingle(actualData);
                            Assert.AreEqual(expectedFloat, actualFloat);
                            break;
                        default:
                            throw new ArgumentException($"Unsupported type: {expectedData.GetType()}");
                    }
                }
                else
                {
                    Assert.AreEqual(expectedData, actualData);
                }
            }
        }


        public static void VerifyCellDataWithWhereClause(this SqlConnection connection, string tableName, 
            string columnName, string whereClause, object expectedData, bool convertData = false)
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            string selectQuery = $"SELECT TOP 1 {columnName} FROM {tableName} WHERE {whereClause}";

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                object actualData = command.ExecuteScalar();

                if (convertData)
                {
                    switch (expectedData)
                    {
                        case int expectedInt:
                            int actualInt = Convert.ToInt32(actualData);
                            Assert.AreEqual(expectedInt, actualInt);
                            break;
                        case string expectedString:
                            string actualString = Convert.ToString(actualData);
                            Assert.AreEqual(expectedString, actualString);
                            break;
                        case float expectedFloat:
                            float actualFloat = Convert.ToSingle(actualData);
                            Assert.AreEqual(expectedFloat, actualFloat);
                            break;
                        default:
                            throw new ArgumentException($"Unsupported type: {expectedData.GetType()}");
                    }
                }
                else
                {
                    Assert.AreEqual(expectedData, actualData);
                }
            }
        }

        public static void UpdateColumnValue(this SqlConnection connection, string tableName, string targetColumnName, object newValue,
            string conditionColumnName, object conditionValue)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));

            if (string.IsNullOrEmpty(conditionColumnName))
                throw new ArgumentException("Condition column name cannot be null or empty.", nameof(conditionColumnName));

            if (string.IsNullOrEmpty(targetColumnName))
                throw new ArgumentException("Target column name cannot be null or empty.", nameof(targetColumnName));

            var query = $"UPDATE {tableName} SET {targetColumnName} = @NewValue WHERE {conditionColumnName} = @ConditionValue";

            using (var command = CreateCommand(connection, query))
            {
                command.Parameters.AddWithValue("@ConditionValue", conditionValue);
                command.Parameters.AddWithValue("@NewValue", newValue);

                command.ExecuteNonQuery();
            }
        }


        public static void DeleteRow(this SqlConnection connection, string tableName, string columnName, object cellData)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            string deleteQuery = $"DELETE FROM {tableName} WHERE {columnName} = @CellData";

            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
            {
                if (cellData is DateTime dateTimeValue)
                {
                    deleteCommand.Parameters.AddWithValue("@CellData", dateTimeValue);
                }
                else if (cellData is int intValue)
                {
                    deleteCommand.Parameters.AddWithValue("@CellData", intValue);
                }
                else if (cellData is string stringValue)
                {
                    deleteCommand.Parameters.AddWithValue("@CellData", stringValue);
                }
                else
                {
                    throw new ArgumentException("Unsupported data type for cellData");
                }

                int rowsDeleted = deleteCommand.ExecuteNonQuery();
                Assert.True(rowsDeleted > 0, "No rows were deleted");
            }
        }



        private static SqlCommand CreateCommand(SqlConnection connection, string query)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return new SqlCommand(query, connection);
        }

        private static SqlCommand CreateInsertCommand(SqlConnection connection, string tableName,
            Dictionary<string, object> data)
        {
            var columns = string.Join(", ", data.Keys);
            var values = string.Join(", ", data.Keys.Select(key => "@" + key));

            var query = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

            var command = CreateCommand(connection, query);

            foreach (var kvp in data)
            {
                command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
            }

            return command;
        }

        public static void PrintQueryResultsTable(this SqlConnection connection, string query)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            if (string.IsNullOrEmpty(query))
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));

            using (var command = CreateCommand(connection, query))
            using (var reader = command.ExecuteReader())
            {
                // Determine column widths
                int[] columnWidths = new int[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columnWidths[i] = Math.Max(reader.GetName(i).Length, 10); 
                }

                // Print column headers
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader.GetName(i).PadRight(columnWidths[i]) + "\t");
                }
                Console.WriteLine();

                // Print data rows
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i].ToString().PadRight(columnWidths[i]) + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }
        public static void PrintQueryResults(this SqlConnection connection, string query)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string columnName = reader.GetName(i);
                            object columnValue = reader[i];

                            Console.Write($"{columnName}: {columnValue}");

                            if (i < reader.FieldCount - 1) 
                            {
                                Console.Write(" | "); 
                            }
                        }
                        Console.WriteLine(); 
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }



    }
}
