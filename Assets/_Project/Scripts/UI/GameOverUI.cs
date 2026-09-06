using TMPro;
using UnityEngine;
namespace WebGLRescueArena { public sealed class GameOverUI : MonoBehaviour { [SerializeField] private GameObject panel; [SerializeField] private TMP_Text finalScoreText; [SerializeField] private TMP_Text bestScoreText; public void Hide() => panel.SetActive(false); public void Show(int score, int bestScore) { panel.SetActive(true); finalScoreText.text = "Score: " + score; bestScoreText.text = "Best: " + bestScore; } } }
