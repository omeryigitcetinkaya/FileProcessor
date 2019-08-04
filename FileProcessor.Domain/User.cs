using System;
using System.Collections.Generic;
using System.Text;

namespace FileProcessor.Domain
{
    public class User
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int RecordID { get; set; }
    }
}
