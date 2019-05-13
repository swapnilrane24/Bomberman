using System.Collections.Generic;
using UnityEngine;
using PlayerSystem;
using EnemySystem;
using Common;
using System.Linq;

namespace LevelSystem
{
    public class LevelController
    {
        public GameObject[,] gridArray;
        private List<Vector2> emptyGridList;
        private int edgeCount = 2;
        private int gridWidth = 15, gridHeight = 11;

        private FixedBlocks fixedBlockPref;
        private BreakableBlocks breakableBlockPref;
        private GameObject levelHolder;
        private IEnemyService enemyService;

        IPlayerService playerService;
        LevelService levelService;

        public LevelController(FixedBlocks fixedBlockPrefab, BreakableBlocks breakableBlockPrefab,
                                IEnemyService enemyService, IPlayerService playerService
                                , LevelService levelService)
        {
            this.playerService = playerService;
            this.enemyService = enemyService;
            this.levelService = levelService;
            this.fixedBlockPref = fixedBlockPrefab;
            this.breakableBlockPref = breakableBlockPrefab;
            ServiceManager.singleton.restartGame += RestartGame;
        }

        ~LevelController()
        {
            ServiceManager.singleton.restartGame -= RestartGame;
        }

        void RestartGame()
        {
            for (int i = 0; i < gridWidth + edgeCount; i++)
            {
                for (int j = 0; j < gridHeight + edgeCount; j++)
                {
                    if (gridArray[i, j] != null)
                    {
                        GameObject obj = gridArray[i, j];
                        Object.Destroy(obj);
                        gridArray[i, j] = null;
                    }
                }
            }

            gridArray = null;
        }

        public void GenerateLevel()
        {
            emptyGridList = new List<Vector2>();
            gridArray = new GameObject[gridWidth + edgeCount, gridHeight + edgeCount];
            if (levelHolder == null)
                levelHolder = new GameObject();
            levelHolder.transform.position = Vector3.zero;
            levelHolder.name = "LevelHolder";

            GenerateGrid();
            GenerateEdgeBoarder();
            GenerateFixedBlock();
            SpawnPlayer();
            GenerateBreakableBlock();
            SpawnEnemies();

            emptyGridList.Clear();
        }

        void GenerateGrid()
        {
            for (int i = 0; i < gridWidth + edgeCount; i++)
            {
                for (int j = 0; j < gridHeight + edgeCount; j++)
                {
                    Vector2 vector = new Vector2(i, j);
                    emptyGridList.Add(vector);
                    gridArray[i, j] = null;
                }
            }
        }

        void GenerateEdgeBoarder()
        {
            for (int i = 0; i < gridHeight + edgeCount; i++)
            {
                Edge(new Vector2(0, i));
                Edge(new Vector2(gridWidth + edgeCount - 1, i));
            }

            for (int i = 1; i < gridWidth + edgeCount - 1; i++)
            {
                Edge(new Vector2(i, 0));
                Edge(new Vector2(i, gridHeight + edgeCount - 1));
            }
        }

        void Edge(Vector2 pos)
        {
            GameObject fixedBlock = Object.Instantiate(fixedBlockPref.gameObject, pos, Quaternion.identity);
            fixedBlock.transform.SetParent(levelHolder.transform);
            fixedBlock.name = "Edge[" + pos.x + "," + pos.y + "]";
            emptyGridList.Remove(pos);
            gridArray[(int)pos.x, (int)pos.y] = fixedBlock;
        }

        void GenerateFixedBlock()
        {
            for (int i = edgeCount; i < gridWidth; i += 2)
            {
                for (int j = edgeCount; j < gridHeight; j += 2)
                {
                    Vector2 vector = new Vector2(i, j);
                    GameObject fixedBlock = Object.Instantiate(fixedBlockPref.gameObject, vector, Quaternion.identity);
                    fixedBlock.transform.SetParent(levelHolder.transform);
                    fixedBlock.name = "Fixed[" + vector.x + "," + vector.y + "]";
                    emptyGridList.Remove(vector);
                    gridArray[(int)vector.x, (int)vector.y] = fixedBlock;
                }
            }
        }

        void SpawnPlayer()
        {
            Vector2 spawnPos = new Vector2(1, gridHeight);
            playerService.SpawnPlayer(spawnPos);
            emptyGridList.Remove(spawnPos);

            for (int i = 1; i < 4; i++)
            {
                for (int j = gridHeight; j > gridHeight - 3; j--)
                {
                    Vector2 tempVector = new Vector2(i, j);
                    emptyGridList.Remove(tempVector);
                }
            }
        }

        void GenerateBreakableBlock()
        {
            int val = Random.Range(Mathf.CeilToInt(emptyGridList.Count / 6), Mathf.CeilToInt(emptyGridList.Count / 3));
            for (int i = 0; i < val; i++)
            {
                int k = Random.Range(0, emptyGridList.Count);
                Vector2 vector = emptyGridList[k];
                GameObject breakableBlock = Object.Instantiate(breakableBlockPref.gameObject, vector, Quaternion.identity);
                breakableBlock.transform.SetParent(levelHolder.transform);
                breakableBlock.name = "Breakable[" + vector.x + "," + vector.y + "]";
                breakableBlock.GetComponent<BreakableBlocks>().SetLevelService(levelService);
                emptyGridList.RemoveAt(k);
                gridArray[(int)vector.x, (int)vector.y] = breakableBlock;
            }
        }

        void SpawnEnemies()
        {
            for (int i = 0; i < 5; i++)
            {
                int k = Random.Range(0, emptyGridList.Count);
                Vector2 vector = emptyGridList[k];
                enemyService.SpawnEnemy(vector);
                emptyGridList.RemoveAt(k);
            }
        }
    }
}