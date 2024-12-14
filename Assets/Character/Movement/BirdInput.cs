using UnityEngine;
using System.Collections; // allows for IEnumerator
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using Unity.VisualScripting;
using UnityEngine.AI;

public class BirdInput : BirdCore {
    void Update() {
        CheckInputs(); 
    }

    void FixedUpdate() {
        HandleWalkInput();
    }

    public void CheckInputs() {
        if (Input.GetKeyDown(KeyCode.Space))
            HandleJumpInput();

        if (Input.GetKeyUp(KeyCode.Space)) 
            HandleReleaseJumpInput();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            HandleDashInput();
    }

    #region movement handlers

    public void HandleWalkInput() {
        if (birdWalk.disableWalk)
            return;

        float direction = Input.GetAxisRaw("Horizontal");
        birdWalk.Walk(direction);
    }

    #region jump

    public void HandleJumpInput() {
        if (TryPerformJump()) 
            return;
        else
            HandleJumpBuffers(); 
    }

    public bool TryPerformJump() {
        if (birdDash.isDashing) 
            return false;

        if (CanPerformFirstJump()) {
            birdJump.FirstJump();
            return true; 
        }
        else if (CanPerformWallJump()) {
            birdJump.WallJump();
            return true;
        }
        else if (CanPerformSecondJump()) {
            birdJump.SecondJump();
            return true;
        }

        return false; 
    }

    public void HandleJumpBuffers() {
        if (birdDash.isDashing) 
            StartCoroutine(birdJump.BufferAnyJump()); 
        else if (ShouldBufferFirstJump()) 
            StartCoroutine(birdJump.BufferFirstJump()); 
    }

    public void HandleReleaseJumpInput() {
        if(birdJump.canApplyDownForce && !birdDash.isDashing) 
            birdJump.TryBufferDownForce();
    }

    #endregion

    void HandleDashInput() {
        if (CanDash())
            StartCoroutine(birdDash.Dash());
    }

    #endregion

    #region condition methods
    private bool ShouldBufferFirstJump() => birdJump.first.hasJumped && birdJump.second.hasJumped;

    private bool CanPerformFirstJump() => (!birdJump.first.hasJumped && birdCollision.isGrounded) || (birdJump.timeSinceOffGround < birdJump.coyoteTime);

    private bool CanPerformWallJump() => !birdCollision.isGrounded && birdCollision.isTouchingWall;

    private bool CanPerformSecondJump() => !birdCollision.isGrounded && !birdJump.second.hasJumped && birdJump.first.timer > 0.05f;

    private bool CanDash() => birdDash.firstDash || (!birdDash.isDashing && birdDash.timeSinceDashEnd > birdDash.dashCooldown);  
    #endregion
}