using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Do.Scripts.Editor
{
    public class ListSceneEditor : EditorWindow
    {
        private Vector2 _scrollPos;
        
        [MenuItem("Tools/List Scene")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ListSceneEditor)).titleContent = new GUIContent("List Scene");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Delete All Pref"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
            GUILayout.Label("List Scene", EditorStyles.boldLabel);
            var scenes = EditorBuildSettings.scenes;
            if (scenes.Length <= 0)
                return;
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,GUILayout.Height(150));
            foreach (var scene in scenes)
            {
                if (!GUILayout.Button(scene.path)) 
                    continue;
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(scene.path);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
