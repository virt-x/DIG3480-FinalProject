using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public string enemyName;
    public int damage, health, xp;
    public bool numbersOff;
    public GameObject[] drops;
    public float[] droprates;
    public PlayerController player;
    private AudioSource damageSound;
    public GameObject deathSound;
    // Start is called before the first frame update
    void Start()
    {
        damageSound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            player.xp += xp;
            for (int i = 0; i < drops.Length; i++)
            {
                if (Random.value < droprates[i])
                {
                    Instantiate(drops[i], transform.position, transform.rotation);
                }
            }
            Instantiate(deathSound, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Attack"))
        {
            damageSound.pitch = Random.Range(0.85f, 1.15f);
            damageSound.Play();
            int damage = collider.GetComponent<AttackType>().magic ? player.intelligence : player.strength;
            damage = Mathf.RoundToInt(damage * collider.GetComponent<AttackType>().multiplier);
            health -= damage;
            player.EnemyDamageNumber(damage, transform.position);
            player.contextBar.ShowContextBar(enemyName, 0);
        }
    }
}
