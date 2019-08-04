using FileProcessor.Console.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace FileProcessor.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(10000);

                List<ServiceReference1.Record> record = new List<ServiceReference1.Record>();
                UserServiceClient client = new UserServiceClient();
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                SqlConnection connection = null;
                SqlDataReader dataReader = null;
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("SELECT * FROM [UserList].[dbo].[Record] WHERE Status = 0", connection);
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        record.Add(new ServiceReference1.Record()
                        {
                            RecordID = Convert.ToInt32(dataReader["RecordID"]),
                            ProcessingTime = Convert.ToDateTime(dataReader["ProcessingTime"]),
                            RowCount = Convert.ToInt32(dataReader["RowCount"]),
                            SuccessRow = Convert.ToInt32(dataReader["SuccessRow"]),                            
                            FileName = Convert.ToString(dataReader["FileName"]),
                            FilePath = Convert.ToString(dataReader["FilePath"]),
                            Status = Convert.ToInt32(dataReader["Status"])
                        });
                    }
                    dataReader.Close();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                foreach (var Record in record)
                {                    
                    var userRecord = client.InsertData(Record);
                    connection.Open();
                    SqlCommand Command = new SqlCommand("UPDATE Record SET ProcessingTime = @ProcessingTime,[RowCount] = '" + userRecord.RowCount + "',SuccessRow = '" + userRecord.SuccessRow + "',FileName = '" + userRecord.FileName + "',FilePath = '" + userRecord.FilePath + "',Status = '" + userRecord.Status + "',ErrorMessage = '"+ userRecord.ErrorMessage +"' WHERE FileName = '" + userRecord.FileName + "'", connection);
                    Command.Parameters.Add("@ProcessingTime", SqlDbType.DateTime).Value = userRecord.ProcessingTime;
                    try
                    {
                        Command.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
