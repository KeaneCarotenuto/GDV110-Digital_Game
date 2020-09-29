using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour 
{

    
    public PlayerControls ctrl;
    public bool IsGrounded;
    public float leavesGroundTime;
    public bool IsJumping;
    public float JumpTime;
    public float defaultJumpTime;
    public float preJumpTime;

    public float MaxSpeed;
    public float Acceleration;
    public float Drag;
    private float MoveDirection;
    public bool IsMoving;

    public bool onWall;

    public Rigidbody2D player;
    public CapsuleCollider2D hitBox;
    public CircleCollider2D groundBox;
    public BoxCollider2D wallBox;
    public SpriteRenderer sRend;

    private Animator anim;



    // Start is called before the first frame update
    void Awake()
    {
        defaultJumpTime = JumpTime;

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

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (onWall && IsMoving && player.velocity.y < 0 )
        {
            player.velocity *= 0.5f;
        }
        else
        {
            
        }

        if (Time.time - leavesGroundTime < 0.2f && Time.time - preJumpTime < 0.1f)
        {
            leavesGroundTime = 0;
            IsGrounded = false;
            IsJumping = true;
            player.AddForce(Vector2.up * 700);
        }

        player.AddForce(Vector2.right * -player.velocity.x * Drag);


        if (JumpTime > 0 && IsJumping)
        {

            float exp = 3.1f;
            player.AddForce(Vector2.up * (Mathf.Exp(-(1 - (JumpTime / defaultJumpTime)) + exp) - Mathf.Exp(-1 + exp)));
        }

        if (JumpTime > 0 && !IsGrounded)
        {
            player.AddForce(Vector2.up * 5);
            JumpTime -= Time.deltaTime;
        }

        if (!IsGrounded)
        {
            player.AddForce(Vector2.up * -30);
        }

        if(IsMoving /*&& IsGrounded*/)
        {
            if(MoveDirection == 1)
            {
                hitBox.offset = new Vector2(Mathf.Abs(hitBox.offset.x), hitBox.offset.y);
                groundBox.offset = new Vector2(Mathf.Abs(groundBox.offset.x), groundBox.offset.y);
                wallBox.offset = new Vector2(Mathf.Abs(wallBox.offset.x), wallBox.offset.y);
                sRend.flipX = false;
                player.AddForce(Vector2.right * Acceleration * (1 - (Mathf.Abs(player.velocity.x) / MaxSpeed)));
            }
            else if(MoveDirection == -1)
            {
                hitBox.offset = new Vector2(-Mathf.Abs(hitBox.offset.x), hitBox.offset.y);
                groundBox.offset = new Vector2(-Mathf.Abs(groundBox.offset.x), groundBox.offset.y);
                wallBox.offset = new Vector2(-Mathf.Abs(wallBox.offset.x), wallBox.offset.y);
                player.gameObject.GetComponent<SpriteRenderer>().flipX = true;
                player.AddForce(Vector2.left * Acceleration * (1- (Mathf.Abs(player.velocity.x) / MaxSpeed)));
            }
        }
    }



    //JUMPING

    public void OnJumpStart(InputAction.CallbackContext ctx)
    {
        preJumpTime = Time.time;

        if (onWall && !IsGrounded)
        {
            player.AddForce(new Vector2(-MoveDirection * 500, 700));
            onWall = false;
        }
    }

    public void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        
    }

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

    //ANIMATIONS

    public void UpdateAnimations()
    {
        anim.SetBool("isMoving", IsMoving);
        anim.SetBool("isGrounded", IsGrounded);
        anim.SetFloat("yVelocity", player.velocity.y);
    }
   
}
