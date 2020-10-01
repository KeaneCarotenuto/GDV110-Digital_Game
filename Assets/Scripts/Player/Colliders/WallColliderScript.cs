using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColliderScript : MonoBehaviour
{
    public PlayerMovement pmScript;

    private void OnTriggerExit2D(Collider2D collision)
    {
        pmScript.onWall = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pmScript.IsGrounded) pmScript.onWall = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pmScript.IsGrounded) pmScript.onWall = true;
    }
}
