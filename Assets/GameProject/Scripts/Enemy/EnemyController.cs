using System.Collections.Generic;
using UnityEngine;
using LevelSystem;
using Common;
using System.Collections;

namespace EnemySystem
{
    public class EnemyController : MonoBehaviour , IDamagable, IKillable
    {
        [SerializeField] private float moveSpeed = 5f;

        ILevelService levelService;
        Vector3 currentGrid, nextGrid;
        List<Vector3> gridPositions;
        float startTime, currentTime, totalDistance;
        EnemyService enemyService;
        [SerializeField] private Transform enemySprite;
        bool canMove, isCaged = true;

        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time;
            currentGrid = nextGrid = transform.position;
            canMove = CanMove();
            if(canMove == true)
            {
                isCaged = false;
                GetNextGrid();
                totalDistance = Vector3.Distance(currentGrid, nextGrid);
            }
            else
            {
                StartCoroutine(CheckIfCaged()); 
            }
            Debug.Log(canMove);
        }

        public void SetServices(ILevelService levelService, EnemyService enemyService)
        {
            this.enemyService = enemyService;
            this.levelService = levelService;
        }

        // Update is called once per frame
        void Update()
        {
            if (isCaged == false)
                Move();
        }

        bool CanMove()
        {
            gridPositions = new List<Vector3>();

            CheckAvailableDirection(Vector3.up);
            CheckAvailableDirection(Vector3.down);
            CheckAvailableDirection(Vector3.left);
            CheckAvailableDirection(Vector3.right);

            if (gridPositions.Count > 0)
            {
                return true;
            }
            return false;
        }

        void GetNextGrid()
        {
            int val = Random.Range(0, gridPositions.Count);
            nextGrid = gridPositions[val];
            LookAT2D(currentGrid, nextGrid);
        }

        private void CheckAvailableDirection(Vector3 direction)
        {
            GameObject obj = levelService.GetObjAtGrid(transform.position + direction);
            if (obj == null || obj.GetComponent<EnemyController>() != null)
            {
                gridPositions.Add(transform.position + direction);
            }
        }

        void Move()
        {
            if(Vector3.Distance(transform.position , nextGrid) > 0.1f)
            {
                currentTime = Time.time;
                float distanceCovered = (currentTime - startTime) * moveSpeed;
                float fraction = distanceCovered / totalDistance;
                transform.position = Vector3.Lerp(currentGrid, nextGrid, fraction);
            }
            else
            {
                transform.position = nextGrid;
                startTime = Time.time;
                currentGrid = nextGrid;
                canMove = CanMove();
                if (canMove)
                {
                    GetNextGrid();
                    totalDistance = Vector3.Distance(currentGrid, nextGrid);
                }
            }
        }

        IEnumerator CheckIfCaged()
        {
            yield return new WaitForSeconds(1f);

            if(CanMove())
            {
                isCaged = false;
                yield return null;
            }

            StartCoroutine(CheckIfCaged());
        }

        public void Damage()
        {
            levelService.EmptyGrid(currentGrid);
            enemyService.RemoveEnemy(this);
            Destroy(gameObject);
        }

        void LookAT2D(Vector2 startPos, Vector2 endPos)
        {
            Vector2 diff = endPos - startPos;
            float zRot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            enemySprite.rotation = Quaternion.Euler(0, 0, zRot + 90);
        }

    }
}