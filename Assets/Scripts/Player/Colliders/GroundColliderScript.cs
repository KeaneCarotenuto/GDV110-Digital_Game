using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderScript : MonoBehaviour
{
    public PlayerMovement pmScript;

    private void OnTriggerExit2D(Collider2D collision)
    {
        pmScript.IsGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time - pmScript.preJumpTime > pmScript.preGroundTime && !collision.isTrigger)
        {
            pmScript.IsGrounded = true;
            pmScript.IsJumping = false;
            pmScript.JumpTime = pmScript.defaultJumpTime;
            pmScript.leavesGroundTime = Time.time;

            pmScript.onWall = false;
        }
        
    }
}
