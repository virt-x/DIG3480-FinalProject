using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamePillar : MonoBehaviour
{
    public GameObject pillar;
    private int pillarSpawnTick = 10;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 8, 0);
        transform.position += Vector3.up * 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pillarSpawnTick--;
        if (pillarSpawnTick == 0)
        {
            RaycastHit2D pillarBase = Physics2D.Raycast(transform.position, Vector2.down, 10, 1 << 8);
            if (pillarBase.point != new Vector2(0,0))
            {
                Instantiate(pillar, pillarBase.point, transform.rotation);
            }
            pillarSpawnTick = 25;
        }
    }
}
