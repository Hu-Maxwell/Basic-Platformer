using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;

public class BirdCollision : BirdCore {
    public LayerMask levelLayer;
    public float groundRayLenX;
    public float groundRayLenY;
    public float wallRayLenX;
    public float wallRayLenY;
    [HideInInspector] public float ignoreGroundCheckTime = 0.05f;
    [HideInInspector] public float originalGravityScale;

    public bool isGrounded = false;
    public bool isTouchingWall = false;

    void Start() {
        levelLayer = LayerMask.GetMask("level");
        originalGravityScale = rb.gravityScale;
    }

    void Update() {
        CheckFloorCollision();
        CheckWallCollision();

        FrictionOnWallManager();
    }

    public void CheckFloorCollision() {
        if (birdJump.first.timer < ignoreGroundCheckTime)
        {
            return;
        }

        RaycastHit2D downRay = Physics2D.BoxCast(transform.position, new Vector2(groundRayLenX, groundRayLenY), 0, Vector2.down, 0, levelLayer); // the vector2.down doesn't matter

        if (downRay)
        {
            isGrounded = true;
            birdJump.curJump = null;

            birdJump.first.hasJumped = false;
            birdJump.second.hasJumped = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void CheckWallCollision() {
        RaycastHit2D sideRay = Physics2D.BoxCast(transform.position, new Vector2(wallRayLenX, wallRayLenY), 0, Vector2.left, 0, levelLayer);

        if (sideRay)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
    }

    public void FrictionOnWallManager() {
        // if is going up or is dashing
        if (birdDash.isDashing || !isTouchingWall) {
            return;
        }
    
        float updatedGravityScale = rb.gravityScale; 
        
        if (rb.linearVelocityY > 0) {
            rb.gravityScale = updatedGravityScale; 
        }
        else if(rb.linearVelocityY < 0) {
            rb.linearVelocityY = 0;
            rb.gravityScale = 0;
            rb.AddForce(new Vector2(0, -3), ForceMode2D.Impulse);
        }
        else if (rb.linearVelocityY == 0) {
            rb.gravityScale = originalGravityScale; 
        }
    }
}