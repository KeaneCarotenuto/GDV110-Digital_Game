using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yayScript : MonoBehaviour
{
    public GameObject text;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Jumper"))
        {
            text.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
