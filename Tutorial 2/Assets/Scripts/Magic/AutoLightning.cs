using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLightning : MonoBehaviour
{
    private Transform target;
    private int retargetTick;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }
        retargetTick++;
        if (retargetTick == 20)
        {
            FindTarget();
            retargetTick = 0;
        }
        body.velocity *= 0.5f;
        body.AddForce((Vector3.Normalize(target.position - transform.position) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0)) * 40, ForceMode2D.Impulse);
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        GameObject[] closeEnemies = new GameObject[enemies.Length];
        int i = 0;
        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) < 20)
            {
                closeEnemies[i] = enemy;
                i++;
            }
        }
        if (i == 0)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        target = closeEnemies[Random.Range(0, i)].transform;
    }
}
