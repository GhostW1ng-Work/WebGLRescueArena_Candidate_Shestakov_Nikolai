using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private HUDController hud;
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private SaveService saveService;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private bool stressMode;

        private int score;
        private static int accumulatedScore;
        private float elapsedTime;
        private float lastHudUpdate;
        private bool ended;

        public bool StressMode => stressMode;

        private void Awake()
        {
            if (saveService == null) saveService = FindObjectOfType<SaveService>();
            if (sceneLoader == null) sceneLoader = FindObjectOfType<SceneLoader>();

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
            if (ended) return;

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

            int bestScore = score;
            if (saveService != null)
            {
                saveService.SaveBestScore(score);
                bestScore = saveService.BestScore;
            }

            gameOverUI.Show(score, bestScore);
            GameEvents.RaiseGameEnded();
        }

        public void Restart()
        {
            if (sceneLoader != null) sceneLoader.RestartGame();
        }

        public void ReturnToMenu()
        {
            if (sceneLoader != null) sceneLoader.LoadMainMenu();
        }
    }
}