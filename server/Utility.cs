using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileWatch.Server
{
    class Utility
    {
        /// <summary>
        /// Returns first file in the directory.
        /// </summary>
        /// <returns></returns>
        public static Tuple<bool,string> GetFirstFile()
        {
            try
            {
                string[] files = Directory.GetFiles(AppConfiguration.DirectoryToMonitor);
                if (files.Length > 0)
                    return new Tuple<bool, string>(true, files[0]);

            }
            catch (Exception)
            {
            }
            return new Tuple<bool, string>(false, string.Empty);
        }
    }
}
