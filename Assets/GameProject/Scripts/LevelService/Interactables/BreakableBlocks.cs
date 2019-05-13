using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

namespace LevelSystem
{
    public class BreakableBlocks : MonoBehaviour, IDamagable
    {
        ILevelService levelService;

        public void SetLevelService(ILevelService levelService)
        {
            this.levelService = levelService;
        }

        public void Damage()
        {
            levelService.EmptyGrid(transform.position);
            Destroy(gameObject);
        }
    }
}