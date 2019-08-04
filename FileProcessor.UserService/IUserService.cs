using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FileProcessor.UserService
{
    
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        List<User> GetAllUsers();

        [OperationContract]
        List<User> GetFilteredUsers(string name, string surname, string telephone, string address);

        [OperationContract]
        Record InsertData(Record record);

        [OperationContract]
        void InsertRecord(Record record);
    }

    [DataContract]
    public class User
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Telephone { get; set; }
        [DataMember]
        public string Address { get; set; }
    }

    [DataContract]
    public class Record
    {
        [DataMember]
        public int RecordID { get; set; }
        [DataMember]
        public DateTime ProcessingTime { get; set; }
        [DataMember]
        public int RowCount { get; set; }
        [DataMember]
        public int SuccessRow { get; set; }                
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }

    }
}
