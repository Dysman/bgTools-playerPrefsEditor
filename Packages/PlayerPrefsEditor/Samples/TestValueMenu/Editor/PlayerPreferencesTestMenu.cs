using UnityEditor;
using UnityEngine;

namespace BgTools.PlayerPrefsEditor
{
    public class PlayerPrefsTestMenu
    {

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Strings", false, 0)]
        public static void addTestValueString()
        {
            PlayerPrefs.SetString("String", "boing");
            PlayerPrefs.SetString("String2", "foo");
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Int", false, 1)]
        public static void addTestValueInt()
        {
            PlayerPrefs.SetInt("Int", 1234);
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Float", false, 2)]
        public static void addTestValueFloat()
        {
            PlayerPrefs.SetFloat("Float", 3.14f);
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry ValidKeys", false, 20)]
        public static void addTestValueAllowedKeyChars()
        {
            PlayerPrefs.SetString(@"1234567890!$%&/()=?`qwertzuiop+asdfghjkl#<yxcvbnm,.-QWERTZUIOP*ASDFGHJKL'>YXCVBNM;:_|", "AllCharsInKey");
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Many", false, 21)]
        public static void addTestValueIntMany()
        {
            for (int i = 0; i < 1000; i++)
            {
                PlayerPrefs.SetInt("Int" + i, i);
            }
            PlayerPrefs.Save();
        }

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/DeleteAll", false, 50)]
        public static void deleteAll()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}