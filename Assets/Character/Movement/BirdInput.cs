using UnityEngine;
using System.Collections; // allows for IEnumerator
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class BirdInput : BirdCore
{
    void Update()
    {
        InputManager(); 
    }

    void FixedUpdate()
    {
        // no if statements needed
        HandleWalkInput();
    }

    public void InputManager()
    {
        if (birdDash.isDashing) 
        {
            return;
        }

        #region jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) && birdJump.canApplyDownForce)
        {
            birdJump.ExertDownForce();
        }
        #endregion

        #region dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
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

    private void HandleJumpInput()
    {
        if (ShouldBufferFirstJump())
        {
            StartCoroutine(birdJump.BufferFirstJump());
        }
        else if (CanPerformFirstJump())
        {
            birdJump.FirstJump();
        }
        else if (CanPerformWallJump())
        {
            birdJump.WallJump();
        }
        else if (CanPerformSecondJump())
        {
            birdJump.SecondJump();
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