#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace DevTools.Utils
{
    public class ImageManager
    {
        private static string GetAssetDir() {

            string pathOfFile = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ImageManager")[0]);
            return pathOfFile.Substring(0, pathOfFile.IndexOf("ImageManager.cs"));
        }

        public static Texture2D GetOsIcon()
        {
#if UNITY_EDITOR_WIN
            return OsWinIcon;
#elif UNITY_EDITOR_OSX
            return OsMacIcon;
#elif UNITY_EDITOR_LINUX
            return OsLinuxIcon;
#endif
        }

        private static Texture2D osLinuxIcon;
        public static Texture2D OsLinuxIcon
        {
            get
            {
                if (osLinuxIcon == null)
                {
                    osLinuxIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "os_linux_icon.png", typeof(Texture2D));
                }
                return osLinuxIcon;
            }
        }

        private static Texture2D osWinIcon;
        public static Texture2D OsWinIcon
        {
            get
            {
                if (osWinIcon == null)
                {
                    osWinIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "os_win_icon.png", typeof(Texture2D));
                }
                return osWinIcon;
            }
        }

        private static Texture2D osMacIcon;
        public static Texture2D OsMacIcon
        {
            get
            {
                if (osMacIcon == null)
                {
                    osMacIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "os_mac_icon.png", typeof(Texture2D));
                }
                return osMacIcon;
            }
        }

        private static Texture2D refresh;
        public static Texture2D Refresh
        {
            get
            {
                if (refresh == null)
                {
                    refresh = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "refresh.png", typeof(Texture2D));
                }
                return refresh;
            }
        }

        private static Texture2D trash;
        public static Texture2D Trash
        {
            get
            {
                if (trash == null)
                {
                    trash = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "trash.png", typeof(Texture2D));
                }
                return trash;
            }
        }

        private static Texture2D exclamation;
        public static Texture2D Exclamation
        {
            get
            {
                if(exclamation == null)
                {
                    exclamation = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "exclamation.png", typeof(Texture2D));
                }
                return exclamation;
            }
        }

        private static Texture2D info;
        public static Texture2D Info
        {
            get
            {
                if (info == null)
                {
                    info = (Texture2D)AssetDatabase.LoadAssetAtPath(GetAssetDir() + "info.png", typeof(Texture2D));
                }
                return info;
            }
        }
    }
}
#endif