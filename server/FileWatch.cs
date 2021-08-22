using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileWatch.Server.Model;
using Microsoft.AspNetCore.SignalR;

namespace FileWatch.Server
{
    class FileWatch
    {       
        private IHubContext<FileHub> _hub { get; set; }
        public FileWatch(IHubContext<FileHub> hub)
        {
            _hub = hub;
        }

        internal void InitFileWatcher(string path, string fileType)
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = @path;
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Filter = "*" + fileType;
            watcher.IncludeSubdirectories = false; // only monitor current directory, disable all sub directory.
            watcher.EnableRaisingEvents = true;
        }

        #region events
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            SendData(new FileWatchModel()
            {
                EventType = e.ChangeType.ToString(),
                FileChangeDateTime = DateTime.Now.ToString(),
                FileName = e.Name
            }).ConfigureAwait(false);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            SendData(new FileWatchModel()
            {
                EventType = "Created",
                FileChangeDateTime = DateTime.Now.ToString(),
                FileName = e.Name
            }).ConfigureAwait(false);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e) {
            SendData(new FileWatchModel()
            {
                EventType = "Deleted",
                FileChangeDateTime = DateTime.Now.ToString(),
                FileName = e.Name
            }).ConfigureAwait(false);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            SendData(new FileWatchModel()
            {
                EventType = "Renamed",
                FileChangeDateTime = DateTime.Now.ToString(),
                FileName = e.Name
            }).ConfigureAwait(false);
        }

        #endregion

        public async Task SendData(FileWatchModel eventData)
        {
            await _hub.Clients.All.SendAsync("messageReceived", eventData);
        }
    }
}
