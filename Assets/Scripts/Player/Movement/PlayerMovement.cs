﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour 
{

    
    public PlayerControls ctrl;

    
    [Header("Jumping")]
    [Tooltip("How Long the Player should move upwards for")] [Range(0.0f, 5.0f)] public float JumpTime;
    [Tooltip("Exponential Force Applied while holding jump")] [Range(0.0f, 5.0f)] public float exp;
    [Tooltip("Pressing space __ seconds before hitting the still lets you jump")] [Range(0.0f, 1.0f)] public float preGroundTime;

    [HideInInspector] public bool IsGrounded;
    [HideInInspector] public float leavesGroundTime;
    [HideInInspector] public bool IsJumping;
    [HideInInspector] public float defaultJumpTime;
    [HideInInspector] public float preJumpTime;
    [HideInInspector] public bool onWall;



    [Header("Moving")]
    [Tooltip("Max Horiz Speed of Player")] [Range(0.0f, 30.0f)] public float MaxSpeed;
    [Tooltip("Horiz Acceleration force")] [Range(0.0f, 100.0f)] public float Acceleration;
    [Tooltip("Horiz Drag force")] [Range(0.0f, 100.0f)] public float Drag;

    [HideInInspector] private float MoveDirection;
    [HideInInspector] public bool IsMoving;



    [Header("Components")]
    public Rigidbody2D player;
    public CapsuleCollider2D hitBox;
    public CircleCollider2D groundBox;
    public BoxCollider2D wallBox;
    public SpriteRenderer sRend;

    private Animator anim;
    



    void Awake()
    {
        //Stores the initial setting, used to reset jump.
        defaultJumpTime = JumpTime;

        IsGrounded = false;
        IsJumping = false;
        leavesGroundTime = -100;
        preJumpTime = -100;

        SetupInput();

        if (player == null) player = GetComponent<Rigidbody2D>();
        if (hitBox == null) hitBox = GetComponentInChildren<CapsuleCollider2D>();
        if (groundBox == null) groundBox = GetComponentInChildren<CircleCollider2D>();
        if (wallBox == null) wallBox = GetComponentInChildren<BoxCollider2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }



    void Update()
    {
        UpdateAnimations();
    }



    void FixedUpdate()
    {
        //Movement
        ApplyDrag();
        LeftRightMovement();

        //Walls
        WallSlide();

        //Jumping
        InitialJump();
        UpwardsForce();
        DownwardsForce();
        HoldingJump();
    }



    #region Setup Methods

    /// <summary>
    /// Used to set up the input functions for the character
    /// </summary>
    void SetupInput()
    {
        ctrl = new PlayerControls();
        ctrl.Player.Move.started += ctx => OnMoveStart(ctx);
        ctrl.Player.Move.performed += ctx => OnMovePerformed(ctx);
        ctrl.Player.Move.canceled += ctx => OnMoveCancelled(ctx);

        ctrl.Player.Jump.started += ctx => OnJumpStart(ctx);
        ctrl.Player.Jump.performed += ctx => OnJumpPerformed(ctx);
        ctrl.Player.Jump.canceled += ctx => OnJumpCancelled(ctx);

        ctrl.Player.ReverseTime.started += ctx => OnReverseTimeStart(ctx);
        ctrl.Player.ReverseTime.performed += ctx => OnReverseTimePerformed(ctx);
        ctrl.Player.ReverseTime.canceled += ctx => OnReverseTimeCancelled(ctx);
        ctrl.Enable();
    }

    #endregion



    #region Movement Methods

    /// <summary>
    /// This moves the player depending on the keys pressed, and flips the player and hit boxes
    /// </summary>
    private void LeftRightMovement()
    {
        if (IsMoving /*&& IsGrounded*/)
        {
            if (MoveDirection == 1)
            {
                MoveRight();
            }

            else if (MoveDirection == -1)
            {
                MoveLeft();
            }
        }
    }

    
    /// <summary>
    /// Moves Player Right...
    /// </summary>
    private void MoveRight()
    {
        hitBox.offset = new Vector2(Mathf.Abs(hitBox.offset.x), hitBox.offset.y);
        groundBox.offset = new Vector2(Mathf.Abs(groundBox.offset.x), groundBox.offset.y);
        wallBox.offset = new Vector2(Mathf.Abs(wallBox.offset.x), wallBox.offset.y);
        sRend.flipX = false;

        //Manages how fast the player can move, and hwo fast they get to that speed.
        player.AddForce(Vector2.right * Acceleration * (1 - (Mathf.Abs(player.velocity.x) / MaxSpeed)));
    }


    /// <summary>
    /// Moves Player Left...
    /// </summary>
    private void MoveLeft()
    {
        hitBox.offset = new Vector2(-Mathf.Abs(hitBox.offset.x), hitBox.offset.y);
        groundBox.offset = new Vector2(-Mathf.Abs(groundBox.offset.x), groundBox.offset.y);
        wallBox.offset = new Vector2(-Mathf.Abs(wallBox.offset.x), wallBox.offset.y);
        player.gameObject.GetComponent<SpriteRenderer>().flipX = true;

        //Manages how fast the player can move, and hwo fast they get to that speed.
        player.AddForce(Vector2.left * Acceleration * (1 - (Mathf.Abs(player.velocity.x) / MaxSpeed)));
    }
    

    /// <summary>
    /// Applies horizontal drag to the player so that they slow down and dont slide
    /// </summary>
    private void ApplyDrag()
    {
        player.AddForce(Vector2.right * -player.velocity.x * Drag);
    }

    #endregion



    #region Wall Movement

    /// <summary>
    /// Used to check if the player should be slowed down while on a wall
    /// </summary>
    private void WallSlide()
    {
        if (onWall && IsMoving && player.velocity.y < 0)
        {
            player.velocity *= 0.5f;
        }
    }


    /// <summary>
    /// If the player is on the wall and they jump, they are pushed away and upwards
    /// </summary>
    private void WallJump()
    {
        if (onWall)
        {
            Debug.Log("Jump" + player.velocity);
            player.AddForce(new Vector2(-MoveDirection * 500, 700));
            onWall = false;
        }
    }

    #endregion



    #region Jumping Methods

    /// <summary>
    /// This is used to apply the initial force of jumping, changing this will change
    /// how high the smallest possible jump is
    /// </summary>
    private void InitialJump()
    {
        if (Time.time - leavesGroundTime < 0.2f && Time.time - preJumpTime < preGroundTime)
        {
            leavesGroundTime = 0;
            IsGrounded = false;
            IsJumping = true;
            player.AddForce(Vector2.up * 700);
        }
    }


    /// <summary>
    /// This force is applied for the duration of the jump to give more upwards force in general
    /// </summary>
    private void UpwardsForce()
    {
        if (JumpTime > 0 && !IsGrounded)
        {
            player.AddForce(Vector2.up * 5);
            JumpTime -= Time.deltaTime;
        }
    }


    /// <summary>
    /// This force is used to slow the player down after a jump, and pull them to the ground if they are airborn
    /// </summary>
    private void DownwardsForce()
    {
        if (!IsGrounded)
        {
            player.AddForce(Vector2.up * -30);
        }
    }


    /// <summary>
    /// When the user continues to hold jump after they have left the ground, they will go slightly higher
    /// </summary>
    private void HoldingJump()
    {
        if (JumpTime > 0 && IsJumping)
        {
            player.AddForce(Vector2.up * (Mathf.Exp(-(1 - (JumpTime / defaultJumpTime)) + exp) - Mathf.Exp(-1 + exp)));
        }
    }

    #endregion



    #region Input Methods

    /// <summary>
    /// When User pressed the jump key
    /// </summary>
    /// <param name="ctx"></param>
    public void OnJumpStart(InputAction.CallbackContext ctx)
    {
        preJumpTime = Time.time;
        WallJump();
    }

    public void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        
    }

    /// <summary>
    /// When user is no longer pressing jump Key
    /// </summary>
    public void OnJumpCancelled(InputAction.CallbackContext ctx)
    {
        IsJumping = false;
    }
    

    //REVERSING TIME
    public void OnReverseTimeStart(InputAction.CallbackContext ctx)
    {

    }

    public void OnReverseTimePerformed(InputAction.CallbackContext ctx)
    {

    }

    public void OnReverseTimeCancelled(InputAction.CallbackContext ctx)
    {

    }


    //MOVING
    public void OnMoveStart(InputAction.CallbackContext ctx)
    {
        
        MoveDirection = ctx.ReadValue<Vector2>().x;
        IsMoving = true;
    }
    public void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        
    }
    public void OnMoveCancelled(InputAction.CallbackContext ctx)
    {
        IsMoving = false;
    }

    #endregion 


    //ANIMATIONS
    public void UpdateAnimations()
    {
        anim.SetBool("isMoving", IsMoving);
        anim.SetBool("isGrounded", IsGrounded);
        anim.SetFloat("yVelocity", player.velocity.y);
        anim.SetBool("onWall", onWall);
    }
   
}
