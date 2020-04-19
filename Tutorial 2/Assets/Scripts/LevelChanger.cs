using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public LevelData targetLevel, currentLevel;
    public Transform targetLocation;
    public Canvas blackScreen;
    public AudioSource musicBox;
    public AudioClip music;
    private PlayerController player;
    private CameraController cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            blackScreen.gameObject.SetActive(true);
            player = collision.collider.GetComponent<PlayerController>();
            player.controlsLocked = true;
            player.hud.gameObject.SetActive(false);
            StartCoroutine(ChangeLevel());
        }
    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        foreach (EnemySpawn spawner in currentLevel.enemySpawns)
        {
            spawner.KillChildren();
        }
        foreach (EnemySpawn spawner in targetLevel.enemySpawns)
        {
            spawner.SpawnEnemy();
        }
        musicBox.Stop();
        player.transform.position = targetLocation.position;
        cam.cameraLimitL = targetLevel.cameraLimitL;
        cam.cameraLimitR = targetLevel.cameraLimitR;
        yield return new WaitForSecondsRealtime(0.25f);
        blackScreen.gameObject.SetActive(false);
        player.GetComponent<PlayerController>().controlsLocked = false;
        player.hud.gameObject.SetActive(true);
        if (music != null)
        {
            musicBox.clip = music;
            musicBox.Play();
        }
    }
}
