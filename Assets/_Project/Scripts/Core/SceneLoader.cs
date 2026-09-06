using UnityEngine;
using UnityEngine.SceneManagement;

namespace WebGLRescueArena
{
    public sealed class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string gameScene = "Game";
        public void LoadMainMenu() => SceneManager.LoadScene(mainMenuScene);
        public void LoadGame() => SceneManager.LoadScene(gameScene);
        public void RestartGame() => SceneManager.LoadScene(gameScene);
    }
}
