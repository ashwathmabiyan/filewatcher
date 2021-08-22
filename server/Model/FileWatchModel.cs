using System;
using System.Collections.Generic;
using System.Text;

namespace FileWatch.Server.Model
{
    class FileWatchModel
    {
        public string FileName { get; set; }
        public string EventType { get; set; }
        public string FileChangeDateTime { get; set; }
    }
}
