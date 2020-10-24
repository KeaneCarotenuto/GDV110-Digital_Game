using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class LedgeTopScript : MonoBehaviour 
{ 
    public PlayerMovement pmScript; 
    public bool isNOTActivated; 
 
    private void FixedUpdate() 
    { 
        isNOTActivated = pmScript.GapOverWall; 
    } 
 
    private void OnTriggerExit2D(Collider2D collision) 
    { 
        pmScript.GapOverWall = true; 
    } 
 
    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        pmScript.GapOverWall = false; 
    } 
 
    private void OnTriggerStay2D(Collider2D collision) 
    { 
        pmScript.GapOverWall = false; 
    } 
} 