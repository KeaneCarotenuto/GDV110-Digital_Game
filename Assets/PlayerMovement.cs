using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour 
{

    
    public PlayerControls ctrl;
    public bool IsGrounded = true;
    public bool IsJumping = false;
    public float JumpTime = 0.5f;
    private float MoveDirection = 0;
    public bool IsMoving = false;
    public Rigidbody2D player;



    // Start is called before the first frame update
    void Awake()
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

    
    void FixedUpdate()
    {
        if(IsJumping && JumpTime > 0 && !IsGrounded)
        {
            player.AddForce(Vector2.up*50);
            JumpTime -= Time.deltaTime;
        }
        if(IsMoving && IsGrounded)
        {
            if(MoveDirection == 1)
            {
                player.AddForce(Vector2.right * 200);
            }
            else if(MoveDirection == -1)
            {
                player.AddForce(Vector2.left * 200);
            }
        }
        
        
    }


    //JUMPING

    public void OnJumpStart(InputAction.CallbackContext ctx)
    {
        if(IsGrounded)
        {
            IsGrounded = false;
            IsJumping = true;
            player.AddForce(Vector2.up*10000);
        }
        Debug.Log("Boing");
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

    //COLLISION

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsGrounded = true;
        JumpTime = 0.5f;
        
    }
   
}
