#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityToolbarExtender;

using Object = UnityEngine.Object;
namespace Utils.Toolbar
{

    [InitializeOnLoad]
    public class ToolbarScene
    {
        private const string ScenesFilePath = "/Scenes";

        static ToolbarScene()
        {
            ToolbarExtender.LeftToolbarGUI.Add(0, OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUIContent content = new GUIContent(SceneManager.GetActiveScene().name);
            Vector2 size = EditorStyles.toolbarDropDown.CalcSize(content);

            string filePath =
                $"{Application.dataPath}{ScenesFilePath}";

            GUILayout.Space(5);

            if (EditorGUILayout.DropdownButton(content, FocusType.Keyboard,
                    EditorStyles.toolbarDropDown, GUILayout.Width(size.x + 5f)) == false) return;

            GenericMenu menu = new();
            MakeSceneMenus(filePath, menu);

            menu.ShowAsContext();
        }

        private static void MakeSceneMenus(string path, GenericMenu menu, string addPath = "")
        {
            string[] scenes = { };
            try
            {
                scenes = Directory.GetFileSystemEntries(path);
            }
            catch
            {
                // ignored
            }
            var guiContent = new GUIContent("[Select SceneFile]");

            if (scenes.Length > 0)
            {
                var filePath = scenes[0];
                filePath = filePath.Replace(Application.dataPath, "Assets");
                // Get the folder as an object
                Object folderObject = AssetDatabase.LoadAssetAtPath(filePath, typeof(Object));

                menu.AddItem(guiContent, false, () =>
                {
                    if (folderObject != null)
                        EditorGUIUtility.PingObject(folderObject);
                });
            }

            foreach (string scene in scenes)
            {
                int dotIndex = scene.LastIndexOf('.');

                string extension = Path.GetFileNameWithoutExtension(scene);

                if (dotIndex == -1)
                {
                    var newPath = $"{addPath}{extension}/";

                    MakeSceneMenus(scene, menu, newPath);
                }
                else
                {
                    string substring = scene[dotIndex..];
                    if (!string.Equals(substring, ".unity", StringComparison.Ordinal)) continue;

                    int assetsIndex = scene.IndexOf("Assets");

                    if (assetsIndex == -1) continue;

                    menu.AddItem(new GUIContent($"{addPath}{extension}"), false, () =>
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                            EditorSceneManager.OpenScene(scene[assetsIndex..]);
                    });
                }
            }
        }
    }
#endif
}