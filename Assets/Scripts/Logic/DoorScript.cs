using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject input;
    public bool open;

    private bool overlapping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        open = input.GetComponent<ButtonScript>().output;

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

        //if (open)
        //{
        //    transform.localScale = Vector3.Lerp(scale, new Vector3(scale.x, 0, scale.z), 0.1f);
        //}
        //else
        //{
        //    transform.localScale = Vector3.Lerp(scale, new Vector3(scale.x, origScale.y, scale.z), 0.1f);
        //}

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        overlapping = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        overlapping = false;
    }
}
