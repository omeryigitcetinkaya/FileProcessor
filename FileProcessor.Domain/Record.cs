using System;
using System.Collections.Generic;
using System.Text;

namespace FileProcessor.Domain
{
    public class Record
    {
        public int RecordID { get; set; }

        public DateTime ProcessingTime { get; set; }

        public int RowCount { get; set; }

        public int SuccessRow { get; set; }
        
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int Status { get; set; }

        public string ErrorMessage { get; set; }

    }
}
