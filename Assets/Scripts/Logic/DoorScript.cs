﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;
    public GameObject input;
    public bool isRuinsVar;


    [HideInInspector] public bool open;

    private bool overlapping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        open = input.GetComponent<ButtonScript>().output;


        if (open)
        {
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().color = Color.gray;
            animator.SetBool("On", !open); 
            animator.SetBool("RuinsVar", isRuinsVar);

        }
        else
        {
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            animator.SetBool("On", !open);
            animator.SetBool("RuinsVar", isRuinsVar); 
        }

    }
}
