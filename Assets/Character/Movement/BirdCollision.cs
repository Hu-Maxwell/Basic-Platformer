using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;

public class BirdCollision : BirdCore
{
    [HideInInspector] public LayerMask levelLayer;
    [HideInInspector] public float rayLenX = 0.525f;
    [HideInInspector] public float rayLenY = 1.005f;
    [HideInInspector] public float ignoreGroundCheckTime = 0.05f;
    [HideInInspector] public float originalGravityScale; 

    void Start()
    {
        levelLayer = LayerMask.GetMask("level");
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        CheckFloorCollision();
        CheckWallCollision();

        FrictionOnWallManager();
    }

    public void CheckFloorCollision()
    {
        if (birdJump.first.timer < ignoreGroundCheckTime)
        {
            return;
        }

        RaycastHit2D downRay = Physics2D.BoxCast(transform.position, new Vector2(0.95f, 2.01f), 0, Vector2.down, 0, levelLayer); // the vector2.down doesn't matter

        if (downRay)
        {
            birdJump.isGrounded = true;
            birdJump.curJump = null;

            birdJump.first.hasJumped = false;
            birdJump.second.hasJumped = false;
        }
        else
        {
            birdJump.isGrounded = false;
        }
    }

    public void CheckWallCollision()
    {
        RaycastHit2D sideRay = Physics2D.BoxCast(transform.position, new Vector2(1.01f, 1), 0, Vector2.left, 0, levelLayer);

        if (sideRay)
        {
            birdJump.isTouchingWall = true;
        }
        else
        {
            birdJump.isTouchingWall = false;
        }
    }

    public void FrictionOnWallManager() 
    {
        // if is going up or is dashing
        if (Math.Sign(rb.linearVelocityY) == 1 || birdDash.isDashing) {
            return;
        }
    
        if (birdJump.isTouchingWall && rb.linearVelocityY != -1) 
        {
            rb.linearVelocityY = 0;
            rb.gravityScale = 0;
            rb.AddForce(new Vector2(0, -3), ForceMode2D.Impulse);
        }
        else
        { 
            rb.gravityScale = originalGravityScale; 
        }
    }
}