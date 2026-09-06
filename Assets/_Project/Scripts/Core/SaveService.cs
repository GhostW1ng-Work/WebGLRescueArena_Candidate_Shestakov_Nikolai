using UnityEngine;

namespace WebGLRescueArena
{
    public sealed class SaveService : MonoBehaviour
    {
        private const string BestScoreKey = "best_score";
        private const string MusicKey = "music_enabled";
        private const string SfxKey = "sfx_enabled";
        public int BestScore => PlayerPrefs.GetInt(BestScoreKey, 0);
        public bool MusicEnabled => PlayerPrefs.GetInt(MusicKey, 1) == 1;
        public bool SfxEnabled => PlayerPrefs.GetInt(SfxKey, 1) == 1;
        public void SaveBestScore(int score) { if (score > BestScore) { PlayerPrefs.SetInt(BestScoreKey, score); PlayerPrefs.Save(); } }
        public void SetMusicEnabled(bool enabled) { PlayerPrefs.SetInt(MusicKey, enabled ? 1 : 0); PlayerPrefs.Save(); }
        public void SetSfxEnabled(bool enabled) { PlayerPrefs.SetInt(SfxKey, enabled ? 1 : 0); PlayerPrefs.Save(); }
    }
}
