using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextBar : MonoBehaviour
{
    public Color enemyColor, itemColor, moneyColor;
    public Text text;
    public Image bar, border;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowContextBar(string barText, int color)
    {
        text.text = barText;
        switch (color)
        {
            case 0:
                bar.color = enemyColor;
                break;
            case 1:
                bar.color = itemColor;
                break;
            case 2:
                bar.color = moneyColor;
                break;
        }
        StopAllCoroutines();
        text.gameObject.SetActive(true);
        bar.gameObject.SetActive(true);
        border.gameObject.SetActive(true);
        StartCoroutine(HideBar());
    }

    IEnumerator HideBar()
    {
        yield return new WaitForSeconds(2);
        text.gameObject.SetActive(false);
        bar.gameObject.SetActive(false);
        border.gameObject.SetActive(false);
    }
}
