using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed, jump;
    private bool floor, doubleJump, doubleEnabled = false;
    public bool controlsLocked = false;
    private Vector3 lastSafePosition;

    public int maxHealth = 100, health = 100,
        maxMana = 100, mana = 100,
        strength = 10, intelligence = 10, constitution = 5,
        level = 1, xp = 0, next = 100,
        money = 0;
    private int mpRegenTick = 0;
    private float invulnTime = 0f;
    private float attackCooldown = 0f;

    public GameObject[] spells = new GameObject[4];
    public bool[] spellsUnlocked = new bool[4];
    public int currentSpell = 0;
    public Transform spellSpawn;

    public Image hpBar, mpBar;
    public Text hpText, mpText;

    public Animator animator;
    public CameraController mainCamera;
    public ContextBar contextBar;
    public Canvas hud;
    public GameObject damageNumber;

    private AudioSource playerNoise;
    public AudioClip jumpSound, attackSound, magicSound, damageSound, levelUpSound;

    public float timeOffset;
    public bool dead;

    // Start is called before the first frame update
    void Start()
    {
        timeOffset = Time.unscaledTime;
        playerNoise = GetComponent<AudioSource>();
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dead && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        if (dead)
        {
            controlsLocked = true;
            hpBar.transform.parent.gameObject.SetActive(false);
            return;
        }
        if (health == 0 && !dead)
        {
            dead = true;
            StartCoroutine(Die());
        }
        if (xp > next - 1)
        {
            LevelUp();
        }
        if (mana < maxMana && !controlsLocked)
        {
            if (mpRegenTick == 0)
            {
                mana++;
                UpdateBars();
                mpRegenTick = Mathf.RoundToInt(60 / (intelligence / 10));
            }
            else
            {
                mpRegenTick--;
            }
        }
        if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Jump")) && !controlsLocked)
        {
            if (floor || doubleJump)
            {
                animator.SetTrigger("Jump");
                if (!floor)
                {
                    doubleJump = false;
                }
                else
                {
                    floor = false;
                }
                PlaySound(0);
                animator.SetBool("Floor", floor);
                //body.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
                body.velocity = new Vector2(body.velocity.x, jump);
            }
        }

        if (Input.GetButtonDown("Attack") && attackCooldown < Time.time && !controlsLocked)
        {
            PlaySound(1);
            animator.SetTrigger("Attack");
            attackCooldown = Time.time + 0.25f;
        }

        if (Input.GetButtonDown("Magic") && attackCooldown < Time.time && !controlsLocked)
        {
            int manaReq = 0;
            switch (currentSpell)
            {
                case 0:
                    manaReq = 10;
                    break;
                case 1:
                    manaReq = 50;
                    break;
                case 2:
                    manaReq = 25;
                    break;
                case 3:
                    manaReq = 75;
                    break;
            }
            if (mana > manaReq - 1)
            {
                PlaySound(2);
                GameObject castSpell = Instantiate(spells[currentSpell], spellSpawn.position, spellSpawn.rotation);
                castSpell.transform.localScale = transform.localScale;
                mana -= manaReq;
                if (mana < 0)
                {
                    mana = 0;
                }
                UpdateBars();
            }
            else
            {
                PlaySound(1);
                StartCoroutine(FlashManaBar());
            }
            animator.SetTrigger("Attack");
            attackCooldown = Time.time + 0.25f;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && !controlsLocked)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && !controlsLocked)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y);
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && !controlsLocked)
        {
            transform.localScale = new Vector3(1, transform.localScale.y);
        }
        //body.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0));
        if (!controlsLocked)
        {
            if (floor)
            {
                body.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, body.velocity.y);
            }
            else
            {
                body.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * speed / 25, 0));
            }
        }
        if (Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1), new Vector2(0.9f, 0.05f), 0f, 1 << 8) != null)
        {
            floor = true;
            if (doubleEnabled)
            {
                doubleJump = true;
            }
            if (body.velocity.magnitude == 0)
            {
                lastSafePosition = transform.position;
            }
        }
        else
        {
            floor = false;
        }
        animator.SetBool("Floor", floor);
        if (Mathf.Abs(body.velocity.x) > speed * 0.75f)
        {
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * speed * 0.75f, body.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Pickup"))
        {
            PickupManager pickup = collider.GetComponent<PickupManager>();
            switch (pickup.type)
            {
                case "Upgrade":
                    doubleEnabled = true;
                    contextBar.ShowContextBar(pickup.pickupName, 1);
                    break;
                case "Magic":
                    spellsUnlocked[pickup.value] = true;
                    contextBar.ShowContextBar(pickup.pickupName, 1);   
                    break;
                case "Money":
                    money += pickup.value;
                    contextBar.ShowContextBar("$" + pickup.value.ToString(), 2);
                    break;
            }
            if (pickup.pickupSound != null)
            {
                Instantiate(pickup.pickupSound, pickup.transform.position, pickup.transform.rotation);
            }
            Destroy(collider.gameObject);
        }
        if (collider.CompareTag("Pit"))
        {
            TakeDamage(Mathf.RoundToInt(maxHealth * 0.25f));
            transform.position = lastSafePosition;
            body.velocity = new Vector2(0, 0);
        }
        if (collider.CompareTag("Enemy"))
        {
            if (invulnTime < Time.time)
            {
                int damage = collider.GetComponent<EnemyController>().damage;
                damage -= Mathf.RoundToInt(constitution / 2f);
                if (damage < 1)
                {
                    damage = 1;
                }
                TakeDamage(damage, true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (invulnTime < Time.time)
            {
                int damage = collision.collider.GetComponent<EnemyController>().damage;
                damage -= Mathf.RoundToInt(constitution / 2f);
                if (damage < 1)
                {
                    damage = 1;
                }
                TakeDamage(damage, true);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Wind"))
        {
            body.AddForce(new Vector2(0, 70));
        }
    }

    void UpdateBars()
    {
        hpBar.transform.localScale = new Vector3((float)health / maxHealth, 1, 1);
        hpText.text = health.ToString();
        mpBar.transform.localScale = new Vector3((float)mana / maxMana, 1, 1);
        mpText.text = mana.ToString();
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
        UpdateBars();
        PlaySound(3);
        invulnTime = Time.time + 0.5f;
        Text damageText = Instantiate(damageNumber, transform.position, transform.rotation, hud.transform).GetComponent<Text>();
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        damageText.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(transform.position); //- new Vector3(cam.pixelHeight / 2, cam.pixelWidth /2, 0));
        damageText.text = damage.ToString();
    }

    void TakeDamage(int damage, bool knockback)
    {
        if (knockback)
        {
            body.AddForce(new Vector2(-10 * transform.localScale.x, 10), ForceMode2D.Impulse);
        }
        TakeDamage(damage);
    }

    public void SetCurrentSpell(int spell)
    {
        currentSpell = spell;
    }

    IEnumerator FlashManaBar()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 1)
            {
                mpBar.color = new Color(0.1215686f, 0.1215686f, 0.4980392f, 0.7529412f);
            }
            else
            {
                mpBar.color = new Color(1, 1, 1, 0.7529412f);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void PlaySound(int sound)
    {
        playerNoise.Stop();
        playerNoise.pitch = Random.Range(0.85f, 1.15f);
        switch (sound)
        {
            case 0:
                playerNoise.clip = jumpSound;
                break;
            case 1:
                playerNoise.clip = attackSound;
                break;
            case 2:
                playerNoise.clip = magicSound;
                break;
            case 3:
                playerNoise.clip = damageSound;
                break;
            case 4:
                playerNoise.pitch = 1;
                playerNoise.clip = levelUpSound;
                break;
        }
        playerNoise.Play();
    }

    public void EnemyDamageNumber(int value, Vector3 pos)
    {
        Text damageText = Instantiate(damageNumber, transform.position, transform.rotation, hud.transform).GetComponent<Text>();
        damageText.color = new Color(1, 0.1f, 0.25f, 1);
        Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        damageText.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(pos); //- new Vector3(cam.pixelHeight / 2, cam.pixelWidth /2, 0));
        damageText.text = value.ToString();
    }

    void LevelUp()
    {
        PlaySound(4);
        level++;
        strength += 2;
        intelligence += 2;
        constitution += 2;
        maxHealth += 10;
        health = maxHealth;
        maxMana += 10;
        mana = maxMana;
        next = Mathf.RoundToInt(next * 1.5f) + 100;
        GetComponent<PopUpTextController>().CreatePopUpTitle("Level Up");
        UpdateBars();
    }

    IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<PopUpTextController>().CreatePopUpTitle("Game Over", "You died. Press ESC to quit, or R to try again.");
        yield return new WaitForSecondsRealtime(1.5f);
        GetComponent<PopUpTextController>().StopAllCoroutines();
        yield break;
    }
}
