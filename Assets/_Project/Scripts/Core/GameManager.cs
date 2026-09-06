using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private HUDController hud;
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private bool stressMode;

        private SaveService saveService;
        private SceneLoader sceneLoader;

        private int score;
        private static int accumulatedScore;
        private float elapsedTime;
        private float lastHudUpdate;
        private bool ended;

        public bool StressMode => stressMode;

        private void Awake()
        {
            saveService = FindObjectOfType<SaveService>();
            sceneLoader = FindObjectOfType<SceneLoader>();

            if (stressMode) enemySpawner.EnableStressMode();
        }

        private void OnEnable()
        {
            GameEvents.EnemyKilled += OnEnemyKilled;
            GameEvents.PlayerDied += OnPlayerDied;
        }

        private void OnDisable()
        {
            GameEvents.EnemyKilled -= OnEnemyKilled;
            GameEvents.PlayerDied -= OnPlayerDied;
        }

        private void Start()
        {
            score = accumulatedScore;
            gameOverUI.Hide();
            GameEvents.RaiseGameStarted();
            UpdateHUD();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime - lastHudUpdate >= 0.1f)
            {
                lastHudUpdate = elapsedTime;
                UpdateHUD();
            }

            if (Input.GetKeyDown(KeyCode.F8))
            {
                enemySpawner.EnableStressMode();
            }
        }

        private void UpdateHUD()
        {
            if (hud != null && playerHealth != null && enemySpawner != null)
            {
                hud.Refresh(score, playerHealth.CurrentHealth, enemySpawner.ActiveEnemyCount, elapsedTime);
            }
        }

        private void OnEnemyKilled(int value)
        {
            accumulatedScore += value;
            score = accumulatedScore;
            GameEvents.RaiseScoreChanged(score);
            UpdateHUD();
        }

        private void OnPlayerDied()
        {
            if (ended) return;
            ended = true;

            if (saveService != null)
            {
                saveService.SaveBestScore(score);
                gameOverUI.Show(score, saveService.BestScore);
            }

            GameEvents.RaiseGameEnded();
        }

        public void Restart() => sceneLoader?.RestartGame();
        public void ReturnToMenu() => sceneLoader?.LoadMainMenu();
    }
}