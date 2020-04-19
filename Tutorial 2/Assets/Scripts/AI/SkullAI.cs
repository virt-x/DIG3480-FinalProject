using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAI : MonoBehaviour
{
    private Rigidbody2D body;
    private EnemyController control;
    private Vector2 direction;
    private int dirTick = 0;
    private bool attackMode;
    public float power = 1f;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        control = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!attackMode && Vector2.Distance(transform.position, control.player.transform.position) < 10 * power)
        {
            attackMode = true;
        }
        if (attackMode)
        {
            body.AddForce(Vector3.Normalize(control.player.transform.position - transform.position) * 5 * power);
        }
        else
        {
            if (dirTick == 0)
            {
                direction = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)).normalized;
                dirTick = Random.Range(45, 300);
            }
            dirTick--;
            body.AddForce(direction * power);
        }
        transform.localScale = new Vector2(Mathf.Sign(body.velocity.x) * -1.5f, 2);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Attack"))
        {
            body.velocity = new Vector2(transform.localScale.x * 3, body.velocity.y * -0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        direction = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)).normalized;
        dirTick = Random.Range(45, 300);
    }
}
