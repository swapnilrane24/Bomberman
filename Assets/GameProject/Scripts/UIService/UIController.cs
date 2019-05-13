using UnityEngine;
using TMPro;
using Common;
using UnityEngine.UI;

namespace UISystem
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameObject gamePanel, gameOverPanel;

        [SerializeField] private TextMeshProUGUI scoreText, goScoreText, goStatusText;
        [SerializeField] private Button restartBtn;

        ServiceManager serviceManager;
        int score;

        public void SetServiceManager(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            this.serviceManager.updateScore += UpdateScore;
            this.serviceManager.gameStatus += UpdateGameStatus;
        }

        private void OnDisable()
        {
            this.serviceManager.updateScore -= UpdateScore;
            this.serviceManager.gameStatus -= UpdateGameStatus;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetUI();
            DontDestroyOnLoad(gameObject);
            restartBtn.onClick.AddListener(RestartGame);
        }

        void UpdateScore()
        {
            score += 10;
            scoreText.text = "SCORE: " + score;
        }

        void UpdateGameStatus(bool gameWon)
        {
            gamePanel.SetActive(false);
            goScoreText.text = "SCORE: " + score;
            goStatusText.text = gameWon == true ? "Won the game!!" : "Lost the game!!";
            gameOverPanel.SetActive(true);
        }

        void RestartGame()
        {
            SetUI();
            serviceManager.RestartGame();
        }

        void SetUI()
        {
            score = 000;
            scoreText.text = "SCORE: " + score;
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
        }

    }
}
