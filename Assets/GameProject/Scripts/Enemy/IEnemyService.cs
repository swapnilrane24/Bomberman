using UnityEngine;
using LevelSystem;

namespace EnemySystem
{
    public interface IEnemyService
    {
        void SpawnEnemy(Vector3 pos);
        void SetLevelService(ILevelService levelService);
    }
}