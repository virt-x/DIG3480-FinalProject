using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearMotionController : MonoBehaviour
{
    public float motion = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.Translate(new Vector2(motion * Time.deltaTime, 0));
        }
    }
}
