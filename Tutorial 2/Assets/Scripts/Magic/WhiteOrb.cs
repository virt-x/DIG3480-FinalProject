using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteOrb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 5, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") || collider.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
