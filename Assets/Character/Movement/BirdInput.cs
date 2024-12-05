using UnityEngine;
using System.Collections; // allows for IEnumerator
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Unity.VisualScripting;

public class BirdInput : BirdCore
{
    void Update()
    {
        InputManager(); 
    }

    void FixedUpdate()
    {
        HandleWalkInput();
    }

    public void InputManager()
    {
        #region jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) && birdJump.canApplyDownForce && !birdDash.isDashing) // change to condition method
        {
            birdJump.TryBufferDownForce();
        }
        #endregion

        // can dash while dashing
        #region dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !birdDash.isDashing)
        {
            HandleDashInput();
        }
        #endregion
    }

    #region movement handlers

    public void HandleWalkInput()
    {
        if (birdWalk.disableWalk)
        {
            return;
        }

        float direction = Input.GetAxisRaw("Horizontal");
        birdWalk.Walk(direction);
    }

    public void HandleJumpInput()
    {
        if (TryPerformJump()) 
        { 
            return;
        } 
        else
        {
            HandleJumpBuffers(); 
        }
    }

    public bool TryPerformJump() {
        if (birdDash.isDashing) {
            return false;
        }

        if (CanPerformFirstJump())
        {
            birdJump.FirstJump();
            return true; 
        }
        else if (CanPerformWallJump())
        {
            birdJump.WallJump();
            return true;
        }
        else if (CanPerformSecondJump())
        {
            birdJump.SecondJump();
            return true;
        }

        return false; 
    }

    public void HandleJumpBuffers() {
        if (birdDash.isDashing) 
        {
            StartCoroutine(birdJump.BufferAnyJump()); 
        }
        else if (ShouldBufferFirstJump()) 
        {
            StartCoroutine(birdJump.BufferFirstJump()); 
        }
    }

    void HandleDashInput()
    {
        if (birdDash.canDash)
        {
            StartCoroutine(birdDash.Dash());
        }
    }

    #endregion

    #region condition methods

    private bool ShouldBufferFirstJump() => birdJump.first.hasJumped && birdJump.second.hasJumped;

    private bool CanPerformFirstJump() => (!birdJump.first.hasJumped && birdJump.isGrounded) || (birdJump.timeSinceOffGround < birdJump.coyoteTime);

    private bool CanPerformWallJump() => !birdJump.isGrounded && birdJump.isTouchingWall;

    private bool CanPerformSecondJump() => !birdJump.isGrounded && !birdJump.second.hasJumped && birdJump.first.timer > 0.05f;
    #endregion
}