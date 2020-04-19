using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    
    public void SpawnEnemy()
    {
        if(transform.childCount == 0)
        {
            Instantiate(enemy, transform.position, transform.rotation, transform);
        }
    }

    public void KillChildren()
    {
        for (int i = transform.childCount - 1; i > -1; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
