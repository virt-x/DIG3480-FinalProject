using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTextController : MonoBehaviour
{
    public Image panel;
    public Text itemTitle, itemDescription, locationTitle;
    public AudioSource music;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePopUpTitle(string locationName)
    {
        locationTitle.gameObject.SetActive(true);
        itemTitle.gameObject.SetActive(false);
        itemDescription.gameObject.SetActive(false);
        locationTitle.text = locationName;
        StartCoroutine(PopUp());
    }
    public void CreatePopUpTitle(string itemName, string itemText)
    {
        locationTitle.gameObject.SetActive(false);
        itemTitle.gameObject.SetActive(true);
        itemDescription.gameObject.SetActive(true);
        itemTitle.text = itemName;
        itemDescription.text = itemText;
        StartCoroutine(PopUp());
    }

    IEnumerator PopUp()
    {
        music.Pause();
        Time.timeScale = 0;
        GetComponent<PlayerController>().controlsLocked = true;
        for (int i = 0; i < 61; i++)
        {
            panel.transform.localScale = new Vector3(1, i / 60f, 1);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(1.5f);
        for (int i = 30; i > -1; i--)
        {
            panel.transform.localScale = new Vector3(1, i / 30f, 1);
            yield return new WaitForEndOfFrame();
        }
        music.UnPause();
        Time.timeScale = 1;
        GetComponent<PlayerController>().controlsLocked = false;
    }
}
