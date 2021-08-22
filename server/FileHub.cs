using FileWatch.Server.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileWatch.Server
{
    class FileHub : Hub
    {
        #region hub actions
        public async Task ChangeFile(string text)
        {
            try
            {
                bool isChanged = Change(text);
                if (!isChanged)
                    await SendMessage("No files found to change").ConfigureAwait(false);
            }
            catch (Exception)
            { 
            }
        }
        public async Task RenameFile(string fileName)
        {
            try
            {
                bool isRenamed = Rename(fileName);
                if (!isRenamed)
                    await SendMessage("No files found to rename").ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }
        public async Task CreateFile(string fileName)
        {
            try
            { 
                Create(fileName); 
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", "Application-Error: " + ex.Message);
            }

        }
        public async Task DeleteFile()
        {
            try
            {
                bool isDeleted = Delete();
                if (!isDeleted)
                    await SendMessage("No files found to delete").ConfigureAwait(false);              
            }
            catch (Exception)
            {
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.Caller.SendAsync("Error", message);
        }

        #endregion

        #region hub functions
        /// <summary>
        /// Create or recreate a file with filename and current DateTime as file
        /// content in the server directory
        /// </summary>
        /// <param name="filename"></param>
        public void Create(string filename)
        {
            try
            {
                string filePath = Path.Combine(AppConfiguration.DirectoryToMonitor, filename);
                
                if (File.Exists(filePath))
                    Delete();

                FileInfo fileInfo = new FileInfo(filePath);
                using (StreamWriter sw = fileInfo.CreateText())
                {
                    StringBuilder sb = new StringBuilder(Path.GetFileNameWithoutExtension(filePath))
                        .Append(DateTime.Now.ToString());
                    sw.WriteLine(sb);
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Delete first file in the server directory
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool Delete()
        {
            bool isDeleted = false;
            try
            {
                Tuple<bool, string> file = Utility.GetFirstFile();

                if (!file.Item1)
                    return isDeleted;

                string filepath = file.Item2;

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
                isDeleted = true;
            }
            catch (Exception)
            {
            }
            return isDeleted;
        }

        /// <summary>
        /// Append text to first file in the server directory
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public bool Change(string text)
        {
            bool isChanged = false;
            try
            {
                Tuple<bool, string> file = Utility.GetFirstFile();

                if (!file.Item1 || string.IsNullOrEmpty(file.Item2))
                    return isChanged;
                 
                string firstFileName = Path.GetFileNameWithoutExtension(file.Item2);
                string newFile = firstFileName + text;

                string oldFile = Path.GetFileName((file.Item2));

                string updatedFile = file.Item2.Replace(oldFile, newFile + AppConfiguration.FileTypeToMonitor);

                FileInfo fileInfo = new FileInfo(file.Item2);

                if (fileInfo.Exists)
                {
                    fileInfo.MoveTo(updatedFile);
                }
                isChanged = true;
            }
            catch (Exception)
            {
            }
            return isChanged;
        }

        /// <summary>
        /// Rename the first file in the server directory to newFilename
        /// </summary>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public bool Rename(string newFileName)
        {
            bool isRenamed = false;
            try
            {
                Tuple<bool, string> file = Utility.GetFirstFile();

                if (!file.Item1)
                    return isRenamed;

                string firstFileName = Path.GetFileNameWithoutExtension(file.Item2); 

                string newFile = file.Item2.Replace(firstFileName, newFileName + AppConfiguration.FileTypeToMonitor);
              
                FileInfo fileInfo = new FileInfo(file.Item2);
              
                if (fileInfo.Exists)
                {
                    fileInfo.MoveTo(newFile);
                }

                isRenamed = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRenamed;
        }        
    }
    #endregion
}
