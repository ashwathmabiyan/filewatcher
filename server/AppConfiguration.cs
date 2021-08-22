using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileWatch.Server
{
    internal class AppConfiguration
    {
        IConfiguration _config;

        public static string FileTypeToMonitor { get; set; }
        public static string DirectoryToMonitor { get; set; }

        public AppConfiguration(IConfiguration config)
        {
            _config = config;
            LoadAppConfigs();
        }

        private void LoadAppConfigs()
        {
            FileTypeToMonitor = _config["FileTypeToMonitor"];
            DirectoryToMonitor = _config["DirectoryToMonitor"];
        }
    }
}
