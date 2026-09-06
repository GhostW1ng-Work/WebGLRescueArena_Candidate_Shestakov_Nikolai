using System;

namespace WebGLRescueArena
{
    public static class GameEvents
    {
        public static event Action<int> EnemyKilled;
        public static event Action<int> PlayerDamaged;
        public static event Action PlayerDied;
        public static event Action<int> ScoreChanged;
        public static event Action GameStarted;
        public static event Action GameEnded;

        public static void RaiseEnemyKilled(int score) => EnemyKilled?.Invoke(score);
        public static void RaisePlayerDamaged(int amount) => PlayerDamaged?.Invoke(amount);
        public static void RaisePlayerDied() => PlayerDied?.Invoke();
        public static void RaiseScoreChanged(int score) => ScoreChanged?.Invoke(score);
        public static void RaiseGameStarted() => GameStarted?.Invoke();
        public static void RaiseGameEnded() => GameEnded?.Invoke();
    }
}
