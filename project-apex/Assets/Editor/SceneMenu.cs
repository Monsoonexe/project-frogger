using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace RichPackage.Editor
{
    public static class SceneMenu
    {
        #region Menu Items

        [MenuItem("Scenes/Reload Current Scene")]
        public static void ReloadCurrentScene()
            => EditorSceneManager.OpenScene(
                SceneManager.GetActiveScene().name); //don't prompt to save

        [MenuItem("Scenes/Dev - Skunkworks")]
        public static void LoadSkunkworks()
            => LoadScene("Assets/Scenes/Dev Scene.unity");

        [MenuItem("Scenes/Levels/Main Menu")]
        public static void LoadLevel_MainMenu()
            => LoadScene("Assets/Scenes/Game Scenes/Main Menu.unity");

        [MenuItem("Scenes/Levels/1")]
        public static void LoadLevel_1()
            => LoadScene("Assets/Scenes/Game Scenes/Level 1.unity");

        [MenuItem("Scenes/Levels/2")]
        public static void LoadLevel_2()
            => LoadScene("Assets/Scenes/Game Scenes/Level 2.unity");

        #endregion

        #region Internal Functions

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(string scenePath)
        {
            //prompt to save
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //actually change scenes
            EditorSceneManager.OpenScene(scenePath);
        }

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(Scene scene)
            => LoadScene(scene.path);

        #endregion

        //----------EXAMPLE ENTRY-----------
        //[MenuItem("Scenes/Start Menu Scene")]
        //private static void LoadStartMenuScene()
        //   => LoadScene("Assets/Scenes/StartMenu.unity");
    }

}
