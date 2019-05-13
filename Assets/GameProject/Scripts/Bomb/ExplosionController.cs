using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private Collider2D colliderRef;
    float spawnTime = 0;

    private void Start()
    {
        Invoke("DestroyObj", 0.4f);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamagable>() != null)
        {
            collision.GetComponent<IDamagable>().Damage();
        }
    }
}
