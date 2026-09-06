using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private SceneLoader sceneLoader;
        private static GameBootstrap instance;
        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start() => sceneLoader.LoadMainMenu();
    }
}
