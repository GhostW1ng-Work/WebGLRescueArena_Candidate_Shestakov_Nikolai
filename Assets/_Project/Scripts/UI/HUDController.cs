using TMPro;
using UnityEngine;
namespace WebGLRescueArena { public sealed class HUDController : MonoBehaviour { [SerializeField] private TMP_Text healthText; [SerializeField] private TMP_Text scoreText; [SerializeField] private TMP_Text enemyCountText; [SerializeField] private TMP_Text timerText; public void Refresh(int score, int health, int enemies, float elapsed) { healthText.text = "HP: " + health; scoreText.text = "Score: " + score; enemyCountText.text = "Enemies: " + enemies; timerText.text = "Time: " + elapsed.ToString("0.0"); } } }
