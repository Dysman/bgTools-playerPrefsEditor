using System;

#if UNITY_EDITOR_WIN
using System.Linq;
using Microsoft.Win32;
#elif UNITY_EDITOR_OSX
using System.IO;
#elif UNITY_EDITOR_LINUX
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
#endif

namespace BgTools.PlayerPreferencesEditor
{
    public abstract class PreferanceStorageAccessor
    {
        protected string prefPath;
        protected string[] cachedData = new string[0];

        protected abstract void FetchKeysFromSystem();

        protected PreferanceStorageAccessor(string pathToPrefs)
        {
            prefPath = pathToPrefs;
        }

        public string[] GetKeys(bool reloadData = true)
        {
            if (reloadData || cachedData.Length == 0)
            {
                FetchKeysFromSystem();
            }

            return cachedData;
        }

        public Action PrefEntryChangedDelegate;
        private bool ignoreNextChange = false;

        public void IgnoreNextChange()
        {
            ignoreNextChange = true;
        }

        protected virtual void OnPrefEntryChanged()
        {
            if (ignoreNextChange)
            {
                ignoreNextChange = false;
                return;
            }

            PrefEntryChangedDelegate();
        }

        public abstract void StartMonitoring();
        public abstract void StopMonitoring();
        public abstract bool IsMonitoring();
    }

#if UNITY_EDITOR_WIN

    public class WindowsPrefStorage : PreferanceStorageAccessor
    {
        RegistryMonitor monitor;

        public WindowsPrefStorage(string pathToPrefs) : base(pathToPrefs)
        {
            monitor = new RegistryMonitor(RegistryHive.CurrentUser, prefPath);
            monitor.RegChanged += new EventHandler(OnRegChanged);
        }

        private void OnRegChanged(object sender, EventArgs e)
        {
            UnityEngine.Debug.Log("registry key has changed time:"+ DateTime.Now);
            OnPrefEntryChanged();
        }

        protected override void FetchKeysFromSystem()
        {
            cachedData = new string[0];

            using (RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(prefPath))
            {
                if (rootKey != null)
                {
                    cachedData = rootKey.GetValueNames();
                }
                rootKey.Close();
            }

            // Clean <key>_h3320113488 nameing
            cachedData = cachedData.Select((key) => { return key.Substring(0, key.IndexOf("_h")); }).ToArray();
        }

        public override void StartMonitoring()
        {
            UnityEngine.Debug.Log("Monitoring: START");

            monitor.Start();
        }

        public override void StopMonitoring()
        {
            UnityEngine.Debug.Log("Monitoring: STOP");

            monitor.Stop();
        }

        public override bool IsMonitoring()
        {
            return monitor.IsMonitoring;
        }

    }

#elif UNITY_EDITOR_LINUX

    public class LinuxPrefStorage : PreferanceStorageAccessor
    {
        FileSystemWatcher fileWatcher;

        public LinuxPrefStorage(string pathToPrefs) : base(Path.Combine(Environment.GetEnvironmentVariable("HOME"), pathToPrefs))
        {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Path.GetDirectoryName(prefPath);
            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
            fileWatcher.Filter = "prefs";

            fileWatcher.Changed += OnWatchedFileChanged;
        }

        protected override void FetchKeysFromSystem()
        {
            cachedData = new string[0];

            if (File.Exists(prefPath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlReader reader = XmlReader.Create(prefPath, settings);

                XDocument doc = XDocument.Load(reader);

                cachedData = doc.Element("unity_prefs").Elements().Select((e) => e.Attribute("name").Value).ToArray();
            }
        }

        public override void StartMonitoring()
        {
            UnityEngine.Debug.Log("Monitoring: START");

            fileWatcher.EnableRaisingEvents = true;
        }

        public override void StopMonitoring()
        {
            UnityEngine.Debug.Log("Monitoring: STOP");

            fileWatcher.EnableRaisingEvents = false;
        }

        public override bool IsMonitoring()
        {
            return fileWatcher.EnableRaisingEvents;
        }

        private void OnWatchedFileChanged(object source, FileSystemEventArgs e)
        {
            UnityEngine.Debug.Log("file changed "+ e.ChangeType.ToString());

            OnPrefEntryChanged();
        }
    }

#elif UNITY_EDITOR_OSX

    public class MacPrefStorage : PreferanceStorageAccessor
    {
        public MacPrefStorage(string pathToPrefs) : base(Path.Combine(Environment.GetEnvironmentVariable("HOME"), pathToPrefs))
        { }

        protected override void FetchKeysFromSystem()
        {
            cachedData = new string[0];

            if (File.Exists(prefPath))
            {
                var cmdStr = string.Format(@"-c ""plutil -convert xml1 {0} && cat {0} | perl -nle 'print $& if m{1}' && plutil -convert binary1 {0}""", prefPath, "{(?<=<key>)(.*?)(?=</key>)}");

                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = "sh";
                process.StartInfo.Arguments = cmdStr;
                process.Start();

                process.WaitForExit();

                cachedData = process.StandardOutput.ReadToEnd().Split('\n').ToArray();
            }
        }
    }
#endif
}