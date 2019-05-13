using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using PlayerSystem;
using EnemySystem;

namespace LevelSystem
{
    public class LevelService : ILevelService
    {
        private LevelController levelController;
        private ServiceManager serviceManager;
        private IPlayerService playerService;

        public LevelService(FixedBlocks fixedBlockPrefab, BreakableBlocks breakableBlockPrefab,
                            IEnemyService enemyService, IPlayerService playerService)
        {
            this.playerService = playerService;
            this.playerService.SetLevelService(this);
            levelController = new LevelController(fixedBlockPrefab, breakableBlockPrefab,
                                                  enemyService, playerService, this);
        }

        public void EmptyGrid(Vector2 position)
        {
            levelController.gridArray[(int)position.x, (int)position.y] = null;
        }

        public void FillGrid(Vector2 position, GameObject gameObject)
        {
            if (levelController.gridArray[(int)position.x, (int)position.y])
                levelController.gridArray[(int)position.x, (int)position.y] = gameObject;
        }

        public void GenerateLevel()
        {
            levelController.GenerateLevel();
        }

        public GameObject GetObjAtGrid(Vector2 position)
        {
            GameObject obj = null;

            if(levelController.gridArray[(int)position.x, (int)position.y])
                obj = levelController.gridArray[(int)position.x, (int)position.y];


            return obj;
        }
    }
}