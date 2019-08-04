using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace FileProcessor.WindowsService
{
    partial class UserService : ServiceBase
    {
        public UserService()
        {
            InitializeComponent();

        }
        //Timer tmr = new Timer();
        private string userFilePath = ConfigurationManager.AppSettings["FilePath"].ToString();
        protected override void OnStart(string[] args)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Filter = "*.*";
            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName;
            fileSystemWatcher.Path = userFilePath;
            fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemCreated);
            fileSystemWatcher.EnableRaisingEvents = true;
            //tmr.Interval = 1000;
            //tmr.Start();
        }

        void fileSystemCreated(object sender, FileSystemEventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(@"C:\Users\ycetinkaya\Desktop\ABC.txt", true);
            streamWriter.WriteLine("* Dosya / Klasör: " + e.Name + " -> " + DateTime.Now.ToString() + " tarihinde oluşturuldu.");
            streamWriter.Close();
        }

        protected override void OnStop()
        {
            
        }
    }
}
