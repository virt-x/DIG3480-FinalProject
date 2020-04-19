using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKiller : MonoBehaviour
{
    public float lifetime;
    private float birth;
    // Start is called before the first frame update
    void Start()
    {
        birth = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (birth + lifetime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
