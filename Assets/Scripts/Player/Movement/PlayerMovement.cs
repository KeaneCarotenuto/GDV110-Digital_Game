using System;
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

public bool IsGrounded;
    [HideInInspector] public float leavesGroundTime;
public bool IsJumping;
    [HideInInspector] public float defaultJumpTime;
    [HideInInspector] public float preJumpTime;
    [HideInInspector] public bool timeReversed;
    [HideInInspector] public float currentTimeScale;

    [HideInInspector] public bool handOnWall;
    [HideInInspector] public bool GapOverWall;
    [HideInInspector] public float preWallJumpTime;
    [Range(0.0f, 1.0f)] public float postWallJumpTime;
    [HideInInspector] public float wallDirection;
 public bool OnLedge;
 public bool onWall;

    [HideInInspector] public bool isDropping;



    [Header("Moving")]
    [Tooltip("Max Horiz Speed of Player")] [Range(0.0f, 30.0f)] public float MaxSpeed;
    [Tooltip("Horiz Acceleration force")] [Range(0.0f, 100.0f)] public float Acceleration;
    [Tooltip("Horiz Drag force")] [Range(0.0f, 100.0f)] public float Drag;

    [HideInInspector] public float MoveDirection;
    [HideInInspector] public bool IsMoving;



    [Header("Components")]
    public Rigidbody2D player;
    public CapsuleCollider2D hitBox;
    public CircleCollider2D groundBox;
    public BoxCollider2D wallBox;
    public SpriteRenderer sRend;
    public TimeManager timeManager;
    public ParticleSystem rewindParticles;
    public GameObject effects;
    private Animator anim;
    



    void Awake()
    {
        //Stores the initial setting, used to reset jump.
        defaultJumpTime = JumpTime;

        IsGrounded = false;
        IsJumping = false;
        GapOverWall = true;
        leavesGroundTime = -100;
        preJumpTime = -100;

        SetupInput();

        if (player == null) player = GetComponent<Rigidbody2D>();
        if (hitBox == null) hitBox = GetComponentInChildren<CapsuleCollider2D>();
        if (groundBox == null) groundBox = GetComponentInChildren<CircleCollider2D>();
        if (wallBox == null) wallBox = GetComponentInChildren<BoxCollider2D>();
        if (anim == null) anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        //Movement
        ApplyDrag();
        LeftRightMovement();

        //Walls
        OnLedgeCheck();
        WallSlide();

        //Jumping
        InitialJump();
        UpwardsForce();
        DownwardsForce();
        HoldingJump();

        //Animation and Effects
        UpdateAnimations();
        PowerEffects();
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

        ctrl.Player.Drop.started += ctx => OnDropStart(ctx);
        ctrl.Player.Drop.performed += ctx => OnDropPerformed(ctx);
        ctrl.Player.Drop.canceled += ctx => OnDropCancelled(ctx);

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
        if (IsMoving && Time.time > preWallJumpTime + postWallJumpTime && !onWall && !OnLedge)
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
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        //Manages how fast the player can move, and hwo fast they get to that speed.
        player.AddForce(Vector2.right * Acceleration * (1 - (Mathf.Abs(player.velocity.x) / MaxSpeed)));
        Debug.Log("Right Force");
    }


    /// <summary>
    /// Moves Player Left...
    /// </summary>
    private void MoveLeft()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        //Manages how fast the player can move, and hwo fast they get to that speed.
        player.AddForce(Vector2.left * Acceleration * (1 - (Mathf.Abs(player.velocity.x) / MaxSpeed)));
        Debug.Log("Left Force");
    }
    

    /// <summary>
    /// Applies horizontal drag to the player so that they slow down and dont slide
    /// </summary>
    private void ApplyDrag()
    {
        if (Time.time > preWallJumpTime + postWallJumpTime)
        {
            player.AddForce(Vector2.right * -player.velocity.x * Drag);
        }
        
    }

    #endregion



    #region Wall Movement

    /// <summary>
    /// Checks to see if the player is on a ledge, then stops them from moving
    /// </summary>
    private void OnLedgeCheck()
    {
        if (!IsGrounded && handOnWall && GapOverWall && onWall && player.velocity.y <= 0)
        {
            OnLedge = true;
        }
        else
        {
            OnLedge = false;
        }

        if (OnLedge == true && !isDropping)
        {
            IsJumping = false;

            player.velocity = new Vector2(player.velocity.x * 0.9f, 0);
            player.gravityScale = 0;
        }
        else
        {
            player.gravityScale = 1;
        }
    }

    /// <summary>
    /// Used to check if the player should be slowed down while on a wall
    /// </summary>
    private void WallSlide()
    {
        if (onWall && player.velocity.y < 0 && !isDropping)
        {
            player.velocity *= 0.9f;
            player.velocity = new Vector2(player.velocity.x * 0.2f, player.velocity.y * 0.5f);
        }
    }


    /// <summary>
    /// If the player is on the wall and they jump, they are pushed away and upwards
    /// </summary>
    private void WallJump()
    {
        if ((onWall || OnLedge) && !IsGrounded)
        {
            preWallJumpTime = Time.time;
            Debug.Log("Jump" + player.velocity);
            float horizDir = (!IsMoving || MoveDirection != wallDirection ? 1 : 0);
            float vertDir = ((wallDirection == MoveDirection && (OnLedge || GapOverWall) ) || wallDirection != MoveDirection || !IsMoving ? 1 : 0);

            player.AddForce(new Vector2(horizDir * -wallDirection * 500, vertDir * 700));
            Debug.Log("Wall Jump Force");
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
        if (Time.time - leavesGroundTime < 0.2f && Time.time - preJumpTime < preGroundTime &&  !onWall)
        {
            leavesGroundTime = 0;
            IsGrounded = false;
            IsJumping = true;
            player.AddForce(Vector2.up * 700);
            Debug.Log("Jump Force");
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
            Debug.Log("Up Force");
            JumpTime -= Time.deltaTime;
        }
    }


    /// <summary>
    /// This force is used to slow the player down after a jump, and pull them to the ground if they are airborn
    /// </summary>
    private void DownwardsForce()
    {
        if (!IsGrounded && !OnLedge)
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

    public void OnJumpCancelled(InputAction.CallbackContext ctx)
    {
        IsJumping = false;
    }

    public void OnDropStart(InputAction.CallbackContext ctx)
    {
        isDropping = true;
    }

    public void OnDropPerformed(InputAction.CallbackContext ctx)
    {
        
    }

    public void OnDropCancelled(InputAction.CallbackContext ctx)
    {
        isDropping = false;
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
        anim.SetBool("onWall", (OnLedge ? false : onWall));
        anim.SetBool("onLedge", OnLedge);
    }

    public void PowerEffects()
    {
        timeReversed = timeManager.timeReversed;
        currentTimeScale = timeManager.timeScale;

        if (currentTimeScale < 0.99 || timeReversed)
        {
            if (rewindParticles.isEmitting)
            {
            return;
            }
            else
            {
            rewindParticles.Play();
            effects.SetActive(true);
            }
        }
        else if(!timeReversed && currentTimeScale >= 0.9)
        {
            rewindParticles.Stop();
            effects.SetActive(false);
            //rewindParticles.Clear();
        }
    }

}
