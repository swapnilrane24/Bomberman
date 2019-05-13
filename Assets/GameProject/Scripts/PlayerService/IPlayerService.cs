using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelSystem;

namespace PlayerSystem
{
    public interface IPlayerService
    {
        void SetLevelService(ILevelService levelService);
        void SpawnPlayer(Vector2 spawnPos);
        GameObject GetPlayer();
    }
}