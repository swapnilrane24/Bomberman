using UnityEngine;
using LevelSystem;
using PlayerSystem;
using EnemySystem;
using System;
using UISystem;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ServiceManager : MonoBehaviour
    {
        public static ServiceManager singleton;

        public event Action<bool> gameStatus;
        public event Action updateScore;
        public event Action restartGame;

        [Range(3,10)]
        public int enemyCount;
        public Vector2 gridSize;
        public UIController uiController;
        public EnemyController enemyPrefab;
        public BombController bombPrefab;
        public PlayerView playerPrefab;
        public FixedBlocks fixedBlock;
        public BreakableBlocks breableBlock;

        ILevelService levelService;
        IPlayerService playerService;
        IEnemyService enemyService;

        private void Awake()
        {
            if(singleton != null)
            {
                Destroy(gameObject); 
            }
            else
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            uiController.SetServiceManager(this);
            playerService = new PlayerService(playerPrefab, bombPrefab, this);
            enemyService = new EnemyService(enemyPrefab, this);
            levelService = new LevelService(fixedBlock, breableBlock, enemyService, playerService);
            enemyService.SetLevelService(levelService);

            levelService.GenerateLevel();
        }

        public void SetGameStatus(bool gameWon) => gameStatus?.Invoke(gameWon); 

        public void UpdateScore() => updateScore?.Invoke();

        public void RestartGame()
        {
            restartGame?.Invoke();
        }

    }
}