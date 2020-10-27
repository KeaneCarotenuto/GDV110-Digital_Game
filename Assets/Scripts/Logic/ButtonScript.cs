using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public Animator animator;
    private GameObject b_press;
    private GameObject b_base;
    public bool isRuinsVar;

    [HideInInspector] public bool output;
    public bool doesToggle;


    // Update is called once per frame
    void Update()
    {
        if (output) //Change to Trigger/Collision
        {
            animator.SetBool("On", !output); 
            animator.SetBool("RuinVar", isRuinsVar);
        }
        else
        {
            animator.SetBool("On", !output); 
            animator.SetBool("RuinVar", isRuinsVar);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        output = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        output = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!doesToggle)
        {
            output = false;
        }
    }
}
