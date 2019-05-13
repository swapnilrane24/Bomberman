using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelSystem;

public class TempGridCreator : MonoBehaviour
{
    [SerializeField]
    private int gridWidth = 15, gridHeight = 10;

    [SerializeField]
    private GameObject fixedBlockPref, breakableBlockPref, enemyPref, playerPref;

    private List<Vector2> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new List<Vector2>();

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                Vector2 vector = new Vector2(i, j);
                grid.Add(vector);
            }
        }

        GenerateFixedBlock();       //O(n2)
        SpawnPlayer();              //O(1)
        GenerateBreakableBlock();   //O(n)
        SpawnEnemies();             //O(n)
    }

    void GenerateFixedBlock()
    {
        for (int i = 1; i < gridWidth; i+=2)
        {
            for (int j = 1; j < gridHeight; j+=2)
            {
                Vector2 vector = new Vector2(i, j);
                GameObject fixedBlock = Instantiate(fixedBlockPref, vector, Quaternion.identity);
                grid.Remove(vector);
            }
        }
    }

    void SpawnPlayer()
    {
        Vector2 vector = new Vector2(0, gridHeight);
        GameObject player = Instantiate(playerPref, vector, Quaternion.identity);
        grid.Remove(vector);

        for (int i = 0; i < 3; i++)
        {
            for (int j = gridHeight; j > gridHeight - 3; j--)
            {
                Vector2 tempVector = new Vector2(i, j);
                grid.Remove(tempVector);
            }
        }

    }

    void GenerateBreakableBlock()
    {
        int val = Random.Range(5, Mathf.CeilToInt(grid.Count / 3));
        for (int i = 0; i < val; i++)
        {
            int k = Random.Range(0, grid.Count);
            Vector2 vector = grid[k];
            GameObject fixedBlock = Instantiate(breakableBlockPref, vector, Quaternion.identity);
            grid.RemoveAt(k);
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            int k = Random.Range(0, grid.Count);
            Vector2 vector = grid[k];
            GameObject enemy = Instantiate(enemyPref, vector, Quaternion.identity);
            grid.RemoveAt(k);
        }
    }

}
