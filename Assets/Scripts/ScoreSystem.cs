using Stack;
using TMPro;
using UnityEngine;

namespace Stack
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        private void Awake()
        {
            AddListener(GameManager.Instance);
        }

        private void AddListener(GameManager instance)
        {
            if (instance != null)
            {
                instance.OnScoreUpdated += HandleOnScoreUpdated;
            }
        }

        private void HandleOnScoreUpdated(int score)
        {
            UpdateScore(score);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString("00");
        }
    }
}
