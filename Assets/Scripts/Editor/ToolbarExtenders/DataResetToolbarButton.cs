using Save;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Editor.ToolbarExtenders
{
    [InitializeOnLoad]
    public class DataResetToolbarButton
    {
        static DataResetToolbarButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            var tex = EditorGUIUtility.IconContent(@"P4_DeletedLocal").image;
            var gui = new GUIContent("Delete Save Data", tex, "Deletes all save data, included player prefs.");
            var buttonStyle = GUI.skin.button;
            var style = new GUIStyle(buttonStyle)
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleRight,
                imagePosition = ImagePosition.ImageLeft,
            };

            if (GUILayout.Button(gui, style))
            {
                DataManager<DataResetToolbarButton>.ClearAll();
                PlayerPrefs.DeleteAll();
            }

            GUILayout.Space(100);
        }
    }
}