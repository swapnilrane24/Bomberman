using UnityEngine;
using LevelSystem;
using Common;

public class BombController : MonoBehaviour
{
    ILevelService levelService;

    [SerializeField] private int explosionArea = 0;
    [SerializeField] private GameObject explosionObj;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 3);
        ServiceManager.singleton.restartGame += RestartGame;
    }

    private void OnDisable()
    {
        ServiceManager.singleton.restartGame -= RestartGame;
    }

    void RestartGame()
    {
        Destroy(gameObject);
    }

    public void SetLevelService(ILevelService levelService)
    {
        this.levelService = levelService;
        this.levelService.FillGrid(transform.position, this.gameObject);
    }

    void Explode()
    {
        ExplodeCell(transform.position, 5, Vector3.zero);
        ExplodeCell(transform.position, 0, Vector3.up);
        ExplodeCell(transform.position, 0, Vector3.left);
        ExplodeCell(transform.position, 0, Vector3.right);
        ExplodeCell(transform.position, 0, Vector3.down);

        levelService.EmptyGrid(transform.position);
        Destroy(gameObject);
    }

    void ExplodeCell(Vector3 position, int areaCovered , Vector3 explosionDirection)
    {
        int area = areaCovered;
        Vector2 targetPos = position + explosionDirection;
        GameObject obj = levelService.GetObjAtGrid(targetPos);

        area++;
        if (obj != null)
        {
            if (obj.GetComponent<FixedBlocks>() != null) return;
            else
            {
                Instantiate(explosionObj, targetPos, Quaternion.identity);
                levelService.EmptyGrid(position);

                if (area < explosionArea)
                {
                    ExplodeCell(transform.position + explosionDirection, area, explosionDirection);
                }
            }
        }
        else
        {
            Instantiate(explosionObj, targetPos, Quaternion.identity);
            if (area < explosionArea)
            {
                ExplodeCell(transform.position + explosionDirection, area, explosionDirection);
            }
        }

    }

}
