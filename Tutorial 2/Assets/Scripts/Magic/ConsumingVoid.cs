using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumingVoid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(Vector3.Normalize(transform.position - enemy.transform.position) * (20 / Vector3.Distance(transform.position, enemy.transform.position)));
        }
    }
}
