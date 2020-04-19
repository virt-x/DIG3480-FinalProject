using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseController : MonoBehaviour
{
    public GameObject hud, magicPanel;
    public EventSystem eventSystem;
    public bool paused;
    private bool alreadyStopped;
    public PlayerController player;
    public Text levelText, xpText, nextText, healthText, manaText, strText, intText, conText, magicText, moneyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && player.health > 0)
        {
            if (!paused && !player.controlsLocked)
            {
                eventSystem.SetSelectedGameObject(transform.GetChild(0).GetChild(0).gameObject);
                UpdatePauseMenu();
                paused = true;
                hud.SetActive(false);
                transform.GetChild(0).gameObject.SetActive(true);
                Time.timeScale = 0;
                if (player.controlsLocked)
                {
                    alreadyStopped = true;
                }
                else
                {
                    player.controlsLocked = true;
                }
            }
            else if (paused)
            {
                paused = false;
                hud.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                Time.timeScale = 1;
                if (alreadyStopped)
                {
                    alreadyStopped = false;
                }
                else
                {
                    player.controlsLocked = false;
                }
            }
        }
    }

    void UpdatePauseMenu()
    {
        levelText.text = "LV. " + player.level.ToString();
        xpText.text = player.xp.ToString();
        nextText.text = (player.next - player.xp).ToString();
        healthText.text = player.health.ToString() + "/" + player.maxHealth.ToString();
        manaText.text = player.mana.ToString() + "/" + player.maxMana.ToString();
        strText.text = player.strength.ToString();
        intText.text = player.intelligence.ToString();
        conText.text = player.constitution.ToString();
        moneyText.text = player.money.ToString();

        SetMagicText(player.currentSpell);

        for (int i = 0; i < player.spellsUnlocked.Length; i++)
        {
            magicPanel.transform.GetChild(i).GetComponent<Button>().interactable = player.spellsUnlocked[i];
            if (player.spellsUnlocked[i])
            {
                switch(i)
                {
                    case 0:
                        magicPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "WHITE ORB";
                        break;
                    case 1:
                        magicPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "FLAME PILLAR";
                        break;
                    case 2:
                        magicPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "AUTOLIGHTNING";
                        break;
                    case 3:
                        magicPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "CONSUMING VOID";
                        break;
                }
            }
            else
            {
                magicPanel.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "???";
            }
        }
    }

    public void Resume()
    {
        paused = false;
        hud.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
        if (alreadyStopped)
        {
            alreadyStopped = false;
        }
        else
        {
            player.controlsLocked = false;
        }
        CloseMagicPanel();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenMagicPanel()
    {
        magicPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(magicPanel.transform.GetChild(player.currentSpell).gameObject);
    }

    public void CloseMagicPanel()
    {
        magicPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(transform.GetChild(0).GetChild(1).gameObject);
    }

    public void SetMagicText(int i)
    {
        switch (i)
        {
            case 0:
                magicText.text = "WHITE ORB";
                break;
            case 1:
                magicText.text = "FLAME PILLAR";
                break;
            case 2:
                magicText.text = "AUTOLIGHTNING";
                break;
            case 3:
                magicText.text = "CONSUMING VOID";
                break;
        }
    }
}
