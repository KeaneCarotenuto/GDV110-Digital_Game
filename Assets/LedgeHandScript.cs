using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeHandScript : MonoBehaviour
{
    public PlayerMovement pmScript;

    private void OnTriggerExit2D(Collider2D collision)
    {
        pmScript.handOnWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pmScript.handOnWall = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        pmScript.handOnWall = true;
    }
}
