// This script will not be attached to an object. So, the class is set as static.
// Also, the class will not be a monoscript.
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scene
    {
        MainMenuScene, // scene names must match with the names of the actual scenes
        GameScene,
        GameOverScene,
    }

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
