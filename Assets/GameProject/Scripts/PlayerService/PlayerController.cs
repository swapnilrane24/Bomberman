using UnityEngine;
using LevelSystem;

namespace PlayerSystem
{
    public class PlayerController
    {
        private PlayerView playerView;
        private PlayerService playerService;
        private GameObject bombPrefab;
        private ILevelService levelService;

        public PlayerView GetPlayerView { get { return playerView; } }

        private GameObject lastBomb = null;

        public PlayerController(PlayerView playerPref, GameObject bombPrefab
                                , Vector2 pos, PlayerService playerService
            , ILevelService levelService)
        {
            this.levelService = levelService;
            this.playerService = playerService;
            this.bombPrefab = bombPrefab;
            GameObject player = Object.Instantiate(playerPref.gameObject, pos, Quaternion.identity);
            playerView = player.GetComponent<PlayerView>();
            playerView.SetController(this);
        }

        public void SpawnBomb()
        {
            Vector2 spawnPOs = playerView.transform.position;
            spawnPOs.x = Mathf.Round(spawnPOs.x);
            spawnPOs.y = Mathf.Round(spawnPOs.y);
            if (lastBomb == null)
            {
                lastBomb = Object.Instantiate(bombPrefab, spawnPOs, Quaternion.identity);
                lastBomb.GetComponent<BombController>().SetLevelService(levelService);
            }
        }

        public void PlayerKilled()
        {
            Object.Destroy(playerView.gameObject);
            playerService.PlayerKilled();
        }

        public void PlayerDestroy()
        {
            Object.Destroy(playerView.gameObject);
        }
    }
}