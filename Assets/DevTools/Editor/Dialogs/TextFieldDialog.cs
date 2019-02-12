using DevTools.Extensions;
using DevTools.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace DevTools.Dialogs
{
    public class TextFieldDialog : EditorWindow
    {
        [NonSerialized]
        private string resultString = string.Empty;

        [NonSerialized]
        private Action<string> callback;

        [NonSerialized]
        private string headerTitle;

        [NonSerialized]
        private string description;

        public static void OpenDialog(string title, string description, Action<string> callback, EditorWindow targetWin = null)
        {
            TextFieldDialog window = ScriptableObject.CreateInstance<TextFieldDialog>();

            window.headerTitle = title;
            window.description = description;
            window.callback = callback;
            window.position = new Rect(0, 0, 350, 140);

            window.CenterOnWindow(targetWin);

            window.ShowPopup();
            window.Focus();
            EditorWindow.FocusWindowIfItsOpen<TextFieldDialog>();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField(headerTitle, GUI.skin.GetStyle("Label"), GUILayout.ExpandWidth(true));
            Styles.HorizontalSeparator();
            GUILayout.Space(10);

            EditorGUILayout.LabelField(description);
            GUILayout.Space(20);

            GUI.SetNextControlName("textField");
            resultString = EditorGUILayout.TextField(resultString, GUILayout.ExpandWidth(true));
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel", GUILayout.Width(75.0f)))
                this.Close();

            if (GUILayout.Button("OK", GUILayout.Width(75.0f)))
            {
                callback(resultString);
                Close();
            }
            GUILayout.EndHorizontal();

            EditorGUI.FocusTextInControl("textField");

            if (Event.current != null && Event.current.isKey)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Return:
                        callback(resultString);
                        Close();
                        break;
                    case KeyCode.Escape:
                        Close();
                        break;
                }

            }
        }
    }
}