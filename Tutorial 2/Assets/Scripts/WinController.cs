using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinController : MonoBehaviour
{
    public AudioSource music;
    public Canvas winCanvas;
    public Text xpTotal, moneyTotal, timeTotal;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            music.loop = false;
            PlayerController player = collider.GetComponent<PlayerController>();
            winCanvas.gameObject.SetActive(true);
            player.dead = true;
            xpTotal.text = player.xp.ToString();
            moneyTotal.text = player.money.ToString();
            int seconds = Mathf.RoundToInt(Time.unscaledTime - player.timeOffset);
            timeTotal.text = Mathf.FloorToInt(seconds / 60f).ToString() + ":" + (seconds % 60).ToString();
        }
    }
}
