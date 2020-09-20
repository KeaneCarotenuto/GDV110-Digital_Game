// These are some generally expected includes that Unity will pre-add for you, generally no need to touch them
using System.Collections;
using System.Collections.Generic;
// This is the most important include (as it gives us access to the UnityEngine structures)
using UnityEngine;

// CLASS 
// Jumper (MonoBehaviour) - inheriting from MonoBehaviour to ensure we can operate within Unity
// DESCRIPTION 
// Has simplistic functionality to allow the player to "jump" towards the mouse cursor
// Can be extended in a variety of ways to enable more interesting behaviours
public class Jumper : MonoBehaviour
{
    // This is an inspector-level element that creates a header in our script inspector in Unity
    // Use these to separate out big blocks of inspector values
    [Header("Jumping")]

    // The tooltip applies to the next inspectable value and provides information when it's hovered in the inspector in Unity
    [Tooltip("The speed that we travel when we jump")] 
    // This is how fast we will travel when we jump in the scene (turn it up to jump faster, down to jump slower)
    public float jumpStrength = 12.0f;

    // The Update function is called every render frame by Unity to handle object processing (can be public/private/protected/etc.)
    private void Update()
    {
        // Read the mouse position from input
        Vector3 mousePos = Input.mousePosition;
        // Zero out our mouse's z as a safety measure
        mousePos.z = 0.0f;

        // Use the camera to transform our screen position to a world position
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        // Set our target z position to our existing z position so that we ensure that we only have difference in x and y
        targetPos.z = transform.position.z;

        // Get the vector between where our mouse is and where we are
        // Then normalize it (magnitude = 1) so that we don't consider how close/far away the mouse is, just what direction
        Vector3 targetDir = (targetPos - transform.position).normalized;

        // If we press the left mouse button (only triggered first frame of pressing)
        if (Input.GetMouseButtonDown(0))
        {
            // Get our Rigidbody2D and set its velocity to towards the mouse at the strength of our jump
            GetComponent<Rigidbody2D>().velocity = new Vector2(targetDir.x, targetDir.y) * jumpStrength;
        }
    }
}
