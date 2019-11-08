#if UNITY_EDITOR_WIN
using System.Linq;
using Microsoft.Win32;
#elif UNITY_EDITOR_OSX
using System;
using System.IO;
#elif UNITY_EDITOR_LINUX
using System;
using System.IO;
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
    }

#if UNITY_EDITOR_WIN

    public class WindowsPrefStorage: PreferanceStorageAccessor
    {
        public WindowsPrefStorage(string pathToPrefs) : base(pathToPrefs)
        { }

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
            cachedData = cachedData.Select((key) => { key = key.Substring(0, key.IndexOf("_h")); return key; }).ToArray();
        }
    }

#elif UNITY_EDITOR_LINUX

    public class LinuxPrefStorage : PreferanceEntryIndexer
    {
        public LinuxPrefStorage(string pathToPrefs) : base(pathToPrefs)
        { }

        protected override void FetchKeysFromSystem()
        {
            cachedData = new string[0];

            string homePath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), prefPath);

            if (File.Exists(homePath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                XmlReader reader = XmlReader.Create(homePath, settings);

                XDocument doc = XDocument.Load(reader);

                cachedData = doc.Element("unity_prefs").Elements().Select((e) => e.Attribute("name").Value).ToArray();
            }
        }
    }

#elif UNITY_EDITOR_OSX

    public class MacPrefStorage : PreferanceEntryIndexer
    {
        public MacPrefStorage(string pathToPrefs) : base(pathToPrefs)
        { }

        protected override void FetchKeysFromSystem()
        {
            cachedData = new string[0];

            string homePath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), prefPath);

            if (File.Exists(homePath))
            {
                var cmdStr = string.Format(@"-c ""plutil -convert xml1 {0} && cat {0} | perl -nle 'print $& if m{1}' && plutil -convert binary1 {0}""", homePath, "{(?<=<key>)(.*?)(?=</key>)}");

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