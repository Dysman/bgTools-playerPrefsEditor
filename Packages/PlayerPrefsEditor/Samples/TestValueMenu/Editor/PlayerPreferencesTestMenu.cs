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

        [MenuItem("Tools/BG Tools/PlayerPrefs Test Values/AddEntry Fonts", false, 22)]
        public static void addTestValueFonts()
        {
            PlayerPrefs.SetString("Arabic (حروف عربية)", "حروف عربية");
            PlayerPrefs.SetString("Cyrillic (Кириллические символы )", "Кириллические символы");
            PlayerPrefs.SetString("Devanagari (देवनागरी चिन्ह)", "देवनागरी चिन्ह");
            PlayerPrefs.SetString("Greek (ελληνικοί χαρακτήρες)", "ελληνικοί χαρακτήρες");
            PlayerPrefs.SetString("Hangul (한글 기호)", "한글 기호");
            PlayerPrefs.SetString("Hànzì (漢字)", "漢字");
            PlayerPrefs.SetString("Hebrew (תווים בעברית)", "תווים בעברית");
            PlayerPrefs.SetString("Hiragana (ひらがな記号)", "ひらがな記号");
            PlayerPrefs.SetString("Katakana (カタカナサイン)", "カタカナサイン");
            PlayerPrefs.SetString("Latin (ingenia Latina)", "ingenia Latina");

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