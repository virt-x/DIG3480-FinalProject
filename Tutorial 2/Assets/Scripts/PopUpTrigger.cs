using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{
    public string itemTitle, itemDescription, locationTitle;
    public bool mode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PopUpTextController pop = collider.GetComponent<PopUpTextController>();
            if (mode)
            {
                pop.CreatePopUpTitle(locationTitle);
                Destroy(gameObject);
            }
            else
            {
                pop.CreatePopUpTitle(itemTitle, itemDescription);
            }
        }
    }
}
