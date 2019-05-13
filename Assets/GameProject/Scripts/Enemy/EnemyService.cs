using System.Collections.Generic;
using UnityEngine;
using LevelSystem;
using Common;

namespace EnemySystem
{
    public class EnemyService : IEnemyService
    {
        private List<EnemyController> enemyControllers;
        private ILevelService levelService;

        private EnemyController enemyPrefab;

        private ServiceManager serviceManager;

        public EnemyService(EnemyController enemyPrefab, ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
            enemyControllers = new List<EnemyController>();
            this.enemyPrefab = enemyPrefab;
            this.serviceManager.restartGame += ResetEnemyList;
        }

        ~EnemyService()
        {
            this.serviceManager.restartGame -= ResetEnemyList;
        }

        void ResetEnemyList()
        {
            for (int i = 0; i < enemyControllers.Count; i++)
            {
                Object.Destroy(enemyControllers[i].gameObject);
            }

            enemyControllers.Clear();
        }

        public void SetLevelService(ILevelService levelService)
        {
            this.levelService = levelService;
        }

        public void SpawnEnemy(Vector3 pos)
        {
            GameObject enemy = GameObject.Instantiate(enemyPrefab.gameObject, pos, Quaternion.identity);
            enemy.GetComponent<EnemyController>().SetServices(levelService, this);
            enemyControllers.Add(enemy.GetComponent<EnemyController>());
        }

        public void RemoveEnemy(EnemyController enemyController)
        {
            enemyControllers.Remove(enemyController);
            serviceManager.UpdateScore();
            if (enemyControllers.Count <= 0)
            {
                //TODO: fire game won event
                serviceManager.SetGameStatus(true);
                Debug.Log("Won Game");
                return;
            }
        }


    }
}