using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR_WIN
using Microsoft.Win32;
#elif UNITY_EDITOR_OSX
using System.Xml.Linq;
using System.Xml;
using UnityEditor.iOS.Xcode;
#endif
using DevTools.Utils;
using DevTools.Dialogs;

namespace DevTools.PreferencesEditor
{
    public class PreferencesEditorWindow : EditorWindow
    {
        #region ErrorValues
        private int ERROR_VALUE_INT = int.MinValue;
        private string ERROR_VALUE_STR = "<devTool_error_24072017>";
        #endregion //ErrorValues

        //private TabState tabState = TabState.PlayerPrefs;
        //private enum TabState
        //{
        //    PlayerPrefs,
        //    EditorPrefs
        //}

        private static string pathToPrefs = String.Empty;

        private string[] userDef;
        private string[] unityDef;
        private bool showSystemGroup = false;

        private SerializedObject serializedObject;
        private ReorderableList userDefList;
        private ReorderableList unityDefList;

        private PreferenceEntryHolder prefEntryHolder;

        Vector2 scrollPos;

        [MenuItem("Tools/DevTools/Preferences Editor", false, 1)]
        static void ShowWindow()
        {
            PreferencesEditorWindow window = EditorWindow.GetWindow<PreferencesEditorWindow>(false, "Prefs Editor");
            window.minSize = new Vector2(400.0f, 300.0f);

            window.InitReorderedList();
            window.prepareData();

            //window.titleContent = EditorGUIUtility.IconContent("SettingsIcon"); // Icon

            window.Show();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR_WIN
            pathToPrefs = @"SOFTWARE\Unity\UnityEditor\" + PlayerSettings.companyName + @"\" + PlayerSettings.productName;
#elif UNITY_EDITOR_OSX
            pathToPrefs = @"Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";
#elif UNITY_EDITOR_LINUX
            pathToPrefs = @"/.config/unity3d/" + PlayerSettings.companyName + "/" + PlayerSettings.productName;
#endif

            // Fix for serialisation issue of static fields
            if (userDefList == null)
                InitReorderedList();
        }

        void InitReorderedList()
        {
            if (prefEntryHolder == null)
            {
                var tmp = Resources.FindObjectsOfTypeAll<PreferenceEntryHolder>();
                if (tmp.Length > 0)
                {
                    prefEntryHolder = tmp[0];
                }
                else
                {
                    prefEntryHolder = ScriptableObject.CreateInstance<PreferenceEntryHolder>();
                }
            }

            if (serializedObject == null)
            {
                serializedObject = new SerializedObject(prefEntryHolder);
            }

            userDefList = new ReorderableList(serializedObject, serializedObject.FindProperty("userDefList"), false, true, true, true);
            unityDefList = new ReorderableList(serializedObject, serializedObject.FindProperty("unityDefList"), false, true, false, false);

            userDefList.onAddCallback = (ReorderableList l) => { Debug.Log("ADD"); };
            userDefList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "User defined");
            };
            userDefList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = userDefList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty key = element.FindPropertyRelative("m_key");
                SerializedProperty type = element.FindPropertyRelative("m_typeSelection");
                SerializedProperty strValue = element.FindPropertyRelative("m_strValue");
                SerializedProperty intValue = element.FindPropertyRelative("m_intValue");
                SerializedProperty floatValue = element.FindPropertyRelative("m_floatValue");
                rect.y += 2;

                EditorGUI.BeginChangeCheck();
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), new GUIContent(key.stringValue, key.stringValue));
                GUI.enabled = false;
                EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y, 60, EditorGUIUtility.singleLineHeight), type, GUIContent.none);
                GUI.enabled = true;
                switch ((PreferenceEntry.PrefTypes)type.enumValueIndex)
                {
                    case PreferenceEntry.PrefTypes.Float:
                        EditorGUI.DelayedFloatField(new Rect(rect.x + 161, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight), floatValue, GUIContent.none);
                        break;
                    case PreferenceEntry.PrefTypes.Int:
                        EditorGUI.DelayedIntField(new Rect(rect.x + 161, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight), intValue, GUIContent.none);
                        break;
                    case PreferenceEntry.PrefTypes.String:
                        EditorGUI.DelayedTextField(new Rect(rect.x + 161, rect.y, rect.width - 160, EditorGUIUtility.singleLineHeight), strValue, GUIContent.none);
                        break;
                }
                if (EditorGUI.EndChangeCheck())
                {
                    switch ((PreferenceEntry.PrefTypes)type.enumValueIndex)
                    {
                        case PreferenceEntry.PrefTypes.Float:
                            PlayerPrefs.SetFloat(key.stringValue, floatValue.floatValue);
                            break;
                        case PreferenceEntry.PrefTypes.Int:
                            PlayerPrefs.SetInt(key.stringValue, intValue.intValue);
                            break;
                        case PreferenceEntry.PrefTypes.String:
                            PlayerPrefs.SetString(key.stringValue, strValue.stringValue);
                            break;
                    }

                    PlayerPrefs.Save();
                }
            };
            userDefList.onRemoveCallback = (ReorderableList l) =>
            {
                // ToDo: remove tabstate if clear that editorprefs not supoorted
                var tabState = "PlayerPrefs";
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this entry from " + tabState + "?", "Yes", "No"))
                {
                    PlayerPrefs.DeleteKey(l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("m_key").stringValue);
                    PlayerPrefs.Save();

                    ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    //reload();
                }
            };

            userDefList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                var menu = new GenericMenu();
                foreach (PreferenceEntry.PrefTypes type in Enum.GetValues(typeof(PreferenceEntry.PrefTypes)))
                {
                    menu.AddItem(new GUIContent(type.ToString()), false, () =>
                    {
                        TextFieldDialog.OpenDialog("Create new property", "Key for the new property:", (key) => {
                            switch (type)
                            {
                                case PreferenceEntry.PrefTypes.Float:
                                    PlayerPrefs.SetFloat(key, 0.0f);

                                    break;
                                case PreferenceEntry.PrefTypes.Int:
                                    PlayerPrefs.SetInt(key, 0);

                                    break;
                                case PreferenceEntry.PrefTypes.String:
                                    PlayerPrefs.SetString(key, string.Empty);

                                    break;
                            }
                            PlayerPrefs.Save();

                            prepareData();

                            Focus();
                        }, this);

                    });
                }
                menu.ShowAsContext();
            };

            unityDefList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = unityDefList.serializedProperty.GetArrayElementAtIndex(index);
                SerializedProperty key = element.FindPropertyRelative("m_key");
                SerializedProperty type = element.FindPropertyRelative("m_typeSelection");
                SerializedProperty strValue = element.FindPropertyRelative("m_strValue");
                SerializedProperty intValue = element.FindPropertyRelative("m_intValue");
                SerializedProperty floatValue = element.FindPropertyRelative("m_floatValue");
                rect.y += 2;

                GUI.enabled = false;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), new GUIContent(key.stringValue, key.stringValue));
                EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y, rect.width - 100 - 231, EditorGUIUtility.singleLineHeight), type, GUIContent.none);

                switch ((PreferenceEntry.PrefTypes)type.enumValueIndex)
                {
                    case PreferenceEntry.PrefTypes.Float:
                        EditorGUI.DelayedFloatField(new Rect(rect.x + rect.width - 229, rect.y, 229, EditorGUIUtility.singleLineHeight), floatValue, GUIContent.none);
                        break;
                    case PreferenceEntry.PrefTypes.Int:
                        EditorGUI.DelayedIntField(new Rect(rect.x + rect.width - 229, rect.y, 229, EditorGUIUtility.singleLineHeight), intValue, GUIContent.none);
                        break;
                    case PreferenceEntry.PrefTypes.String:
                        EditorGUI.DelayedTextField(new Rect(rect.x + rect.width - 229, rect.y, 229, EditorGUIUtility.singleLineHeight), strValue, GUIContent.none);
                        break;
                }
                GUI.enabled = true;
            };
            unityDefList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Unity defined");
            };
        }

        void OnGUI()
        {
            Color defaultColor = GUI.contentColor;

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            GUI.contentColor = (EditorGUIUtility.isProSkin) ? Styles.Colors.LightGray : Styles.Colors.DarkGray;
            GUILayout.Box(ImageManager.GetOsIcon(), Styles.icon);
            GUI.contentColor = defaultColor;

            GUILayout.TextField(@"<CurrendUser>" + Path.DirectorySeparatorChar + pathToPrefs, GUILayout.MinWidth(200)); 

            GUI.contentColor = (EditorGUIUtility.isProSkin) ? Styles.Colors.LightGray : Styles.Colors.DarkGray;
            if (GUILayout.Button(new GUIContent(ImageManager.Refresh, "Refresh"), Styles.miniButton))
            {
                prepareData();
            }
            if (GUILayout.Button(new GUIContent(ImageManager.Trash, "Delete all"), Styles.miniButton))
            {
                // ToDo: remove tabstate if clear that editorprefs not supoorted
                var tabState = "PlayerPrefs";
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete ALL entries from " + tabState + "?\n\nUse with caution! Unity defined keys are affected too.", "Yes", "No"))
                {
                    PlayerPrefs.DeleteAll();
                }
            }
            GUI.contentColor = defaultColor;

            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();

            //if (GUILayout.Toggle(tabState == TabState.PlayerPrefs, "PlayerPrefs", EditorStyles.toolbarButton))
            //    tabState = TabState.PlayerPrefs;

            //GUI.enabled = false;
            //if (GUILayout.Toggle(tabState == TabState.EditorPrefs, "EditorPrefs", EditorStyles.toolbarButton))
            //    tabState = TabState.EditorPrefs;
            //GUI.enabled = true;

            //GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos);
            serializedObject.Update();
            userDefList.DoLayoutList(); 
            serializedObject.ApplyModifiedProperties();

            GUILayout.FlexibleSpace();

            showSystemGroup = EditorGUILayout.Foldout(showSystemGroup, new GUIContent("Show System"));
            if (showSystemGroup)
            {
                unityDefList.DoLayoutList();
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void prepareData()
        {
            prefEntryHolder.ClearLists();

            loadKeys(out userDef, out unityDef);

            CreatePrefEntries(userDef, prefEntryHolder.userDefList);
            CreatePrefEntries(unityDef, prefEntryHolder.unityDefList);
        }

        private void CreatePrefEntries(string[] keySource, List<PreferenceEntry> listDest)
        {
            foreach (string key in keySource)
            {
                var entry = new PreferenceEntry();
                entry.m_key = key;

                string s = PlayerPrefs.GetString(key, ERROR_VALUE_STR);

                if (s != ERROR_VALUE_STR)
                {
                    entry.m_strValue = s;
                    entry.m_typeSelection = PreferenceEntry.PrefTypes.String;
                    listDest.Add(entry);
                    continue;
                }

                float f = PlayerPrefs.GetFloat(key, float.NaN);
                if (!float.IsNaN(f))
                {
                    entry.m_floatValue = f;
                    entry.m_typeSelection = PreferenceEntry.PrefTypes.Float;
                    listDest.Add(entry);
                    continue;
                }

                int i = PlayerPrefs.GetInt(key, ERROR_VALUE_INT);
                if (i != ERROR_VALUE_INT)
                {
                    entry.m_intValue = i;
                    entry.m_typeSelection = PreferenceEntry.PrefTypes.Int;
                    listDest.Add(entry);
                    continue;
                }
            }
        }

        private void loadKeys(out string[] userDef, out string[] unityDef)
        {
            string[] keys = new string[0];

#if UNITY_EDITOR_WIN
            using (RegistryKey rootKey = Registry.CurrentUser.OpenSubKey(pathToPrefs))
            {
                if (rootKey != null)
                {
                    keys = rootKey.GetValueNames();
                }
                rootKey.Close();
            }

            // Clean <key>_h3320113488 nameing
            keys = keys.Select( (key) => { key = key.Substring(0, key.IndexOf("_h")); return key; } ).ToArray();

#elif UNITY_EDITOR_OSX
            string homePath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), pathToPrefs);
            string tmpPath = Path.Combine(Environment.GetEnvironmentVariable("HOME"), "tmpDevToolsPlayerPrefsEncodet.plist");


            if(File.Exists(homePath)){
	            var cmdStr = string.Format(@"-c ""plutil -convert xml1 {0} && cat {0} | perl -nle 'print $& if m{1}' && plutil -convert binary1 {0}""", homePath, "{(?<=<key>)(.*?)(?=</key>)}");

                var process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.FileName = "sh";
                process.StartInfo.Arguments = cmdStr;
                process.Start();
    
                process.WaitForExit();
    
                keys = process.StandardOutput.ReadToEnd().Split('\n').ToArray();
    
                // var process = System.Diagnostics.Process.Start("plutil", "-convert xml1 " + homePath + " -o " + tmpPath);
                // process.WaitForExit();

/*              // Plist not working unity define it as malformed
                PlistDocument plist = new PlistDocument();
                plist.ReadFromFile(tmpPath);
                PlistElementDict rootDict = plist.root;
                keys = rootDict.values.Keys.ToArray();
*/
            // Fix issue of Document Type Declaration (DTD) is prohibited
            /*     XmlReaderSettings settings = new XmlReaderSettings();
                settings.ProhibitDtd = false;
                XmlReader reader = XmlReader.Create(tmpPath, settings);
    
                XDocument doc = XDocument.Load(reader);
                XElement plist = doc.Element("plist");
                XElement dict = plist.Element("dict");
                IEnumerable<XElement> tags = dict.Elements();

                keys = tags.Where( (a) => a.Name.ToString() == "key").Select( (e) => e.Value ).ToArray();

                process = System.Diagnostics.Process.Start("rm", tmpPath);
                process.WaitForExit();
*/          }
#endif

            // ToDo: implement and Linux

            // Seperate keys int unity defined and user defined
            var groups = keys.GroupBy((key) => key.StartsWith("unity.") || key.StartsWith("UnityGraphicsQuality"))
                      .ToDictionary( (g) => g.Key, (g) => g.ToList());

            unityDef = (groups.ContainsKey(true)) ? groups[true].ToArray() : new string[0];
            userDef = (groups.ContainsKey(false)) ? groups[false].ToArray() : new string[0];
        }
    }
}