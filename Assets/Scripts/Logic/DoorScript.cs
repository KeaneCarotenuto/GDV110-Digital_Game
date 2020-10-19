using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject input;
    public bool staysOpen;


    [HideInInspector] public bool open;

    private bool overlapping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!open)
        {
            open = input.GetComponent<ButtonScript>().output;
        }
        

        if (open || overlapping)
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().color = Color.gray;

        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }
}
