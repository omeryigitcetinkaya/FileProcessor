using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.Console
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
