using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FileProcessor.UserService
{
    public class UserService : IUserService
    {
        private string userFilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        private string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private SqlConnection connection = null;

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            SqlDataReader dataReader = null;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("SELECT * FROM [UserList].[dbo].[UserList]", connection);

            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    users.Add(new User()
                    {
                        Name = dataReader[0].ToString(),
                        Surname = dataReader[1].ToString(),
                        Telephone = dataReader[2].ToString(),
                        Address = dataReader[3].ToString()
                    });
                }

                connection.Close();

                return users;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<User> GetFilteredUsers(string name, string surname, string telephone, string address)
        {
            List<User> users = new List<User>();
            string query = "select * FROM [UserList].[dbo].[UserList] where 1=1";
            SqlDataReader dataReader = null;
            connection = new SqlConnection(connectionString);

            if (!String.IsNullOrEmpty(name))
                query += " and (Name LIKE '%' + @Name + '%')";

            if (!String.IsNullOrEmpty(surname))
                query += " and (Surname LIKE '%' + @Surname + '%')";

            if (!String.IsNullOrEmpty(telephone))
                query += " and (Telephone LIKE '%' + @Telephone + '%')";

            if (!String.IsNullOrEmpty(address))
                query += " and (Address LIKE '%' + @Address + '%')";

            SqlCommand command = new SqlCommand(query, connection);


            if (!String.IsNullOrEmpty(name))
            {
                command.Parameters.AddWithValue("@Name", name);
            }
            if (!String.IsNullOrEmpty(surname))
            {
                command.Parameters.AddWithValue("@Surname", surname);
            }
            if (!String.IsNullOrEmpty(telephone))
            {
                command.Parameters.AddWithValue("@Telephone", telephone);
            }
            if (!String.IsNullOrEmpty(address))
            {
                command.Parameters.AddWithValue("@Address", address);
            }
            connection.Open();
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                User u = new User
                {
                    Name = dataReader[1].ToString(),
                    Surname = dataReader[2].ToString(),
                    Telephone = dataReader[3].ToString(),
                    Address = dataReader[4].ToString()
                };
                users.Add(u);
            }
            dataReader.Close();
            connection.Close();
            return users;
        }

        public Record InsertData(Record record)
        {
            int successRow = 0;
            if (!Path.GetExtension(record.FilePath).Contains("txt"))
            {
                record.FileName = Path.GetFileName(record.FilePath);
                record.ProcessingTime = DateTime.Now;
                record.Status = 2;
                record.ErrorMessage = " Dosya txt dosyası değildir. ";
                record.ProcessingTime = DateTime.Now;
                return record;
            }

            try
            {
                string userText = File.ReadAllText(record.FilePath, Encoding.Default);
                string rowDelimiter = @"\r\n";
                string[] userRow = Regex.Split(userText, rowDelimiter);

                DataTable dataTable = new DataTable();
                dataTable.CaseSensitive = false;
                dataTable.Columns.Add(new DataColumn("Name"));
                dataTable.Columns.Add(new DataColumn("Surname"));
                dataTable.Columns.Add(new DataColumn("Telephone"));
                dataTable.Columns.Add(new DataColumn("Address"));
                dataTable.Columns.Add(new DataColumn("RecordID"));

                int index = 0;
                int errorCount = 0;
                bool hasError = false;
                record.ErrorMessage = "Bazı satırlar hatalı.Hatalı satırlar ; ";
                foreach (string row in userRow)
                {
                    string rw = row;
                    rw += "&" + record.RecordID.ToString();
                    string userInfoDelimiter = @"&";
                    if (row.Split('&').Length < (dataTable.Columns.Count - 1))
                    {
                        hasError = true;
                        errorCount++;
                        if (errorCount < 10)
                        {
                            record.ErrorMessage += (index+1).ToString() + ",";
                        }                        
                        index++;
                        continue;
                    }
                    index++;
                    string[] userInfo = Regex.Split(rw, userInfoDelimiter);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow.ItemArray = userInfo;
                    dataTable.Rows.Add(dataRow);
                    successRow++;                    
                }
                if (errorCount >= 10)
                {
                    record.ErrorMessage += " ve daha fazla satırda hata var. ";
                }                
                if (!hasError)
                {
                    record.ErrorMessage = "";
                }

                if (successRow == 0)
                {
                    record.Status = 2;
                    record.ErrorMessage = "Bütün satırlar hatalı.";
                    record.ProcessingTime = DateTime.Now;
                    return record;
                }

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
                {
                    sqlBulkCopy.DestinationTableName = "[UserList].[dbo].[UserList]";

                    sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                    sqlBulkCopy.ColumnMappings.Add("Surname", "Surname");
                    sqlBulkCopy.ColumnMappings.Add("Telephone", "Telephone");
                    sqlBulkCopy.ColumnMappings.Add("Address", "Address");
                    sqlBulkCopy.ColumnMappings.Add("RecordID", "RecordID");
                    sqlBulkCopy.WriteToServer(dataTable);
                }

                record.RowCount = userRow.Length;
                record.ProcessingTime = DateTime.Now;
                record.FileName = Path.GetFileName(record.FilePath);
                record.SuccessRow = successRow;
                record.Status = 1;                
                return record;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void InsertRecord(Record record)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection connection = null;
            try
            {
                record.FileName = Path.GetFileName(record.FilePath);
                record.ProcessingTime = DateTime.Now;
                record.Status = 0;
                connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Record(ProcessingTime,[RowCount],SuccessRow,FileName,FilePath,Status) VALUES(@ProcessingTime,'" + record.RowCount + "','" + record.SuccessRow + "','" + record.FileName + "','" + record.FilePath + "','" + record.Status + "')", connection);
                command.Parameters.Add("@ProcessingTime", SqlDbType.DateTime).Value = record.ProcessingTime;
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                throw;
            }
        }
    }
}
