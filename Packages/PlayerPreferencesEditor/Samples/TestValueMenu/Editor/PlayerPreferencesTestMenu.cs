using UnityEditor;
using UnityEngine;

namespace BgTools.PlayerPrefsEditor
{
    public class PlayerPrefsTestMenu
    {

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Strings")]
        public static void addTestValueString()
        {
            PlayerPrefs.SetString("String", "boing");
            PlayerPrefs.SetString("String2", "foo");
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Int")]
        public static void addTestValueInt()
        {
            PlayerPrefs.SetInt("Int", 1234);
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Float")]
        public static void addTestValueFloat()
        {
            PlayerPrefs.SetFloat("Float", 3.14f);
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/DeleteAll")]
        public static void deleteAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}