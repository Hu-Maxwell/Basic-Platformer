using System.Collections.Generic; 
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering;
using System.ComponentModel.Design;

[System.Serializable]
public class Jump {
    public string name;
    public Vector2 force;
    public float timer;
    public bool hasJumped;
}

public class BirdJump : BirdCore {
    #region vars 
    public Jump first;
    public Jump second;
    public Jump wall;

    List<Jump> jumps;
    [HideInInspector] public Jump curJump;

    [HideInInspector] public float jumpDownVel = -2.0f;
    [HideInInspector] public float toApexTime = 0; 
    [HideInInspector] public float timeSinceOffGround;

    [HideInInspector] public bool canApplyDownForce = true;
    public bool canLowerGravity = true; 

    [HideInInspector] public float coyoteTime = 0.1f;
    [HideInInspector] public float waitUntilDownForceTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    public float firstJumpBuffer = 0.03f;
    [HideInInspector] public float originalGravityScale; 

    [HideInInspector] public bool disableWalk = false;
    [HideInInspector] public float disableWalkTime = 0.15f;
    #endregion
    
    void Start() {
        InitializeJumpVariables(); 
    }

    void Update() {
        UpdateTimers();

        LowerGravityAtApex();
        UpdateDownForceState();
    }

    #region jumps

    private void PerformJump(Jump jump, Vector2 extraForce) {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); 
        rb.AddForce(jump.force + extraForce, ForceMode2D.Impulse);

        jump.hasJumped = true;
        jump.timer = 0;

        canApplyDownForce = true;
        canLowerGravity = true; 
        curJump = jump;
    }

    public void FirstJump() => PerformJump(first, Vector2.zero);
    public void SecondJump() => PerformJump(second, Vector2.zero);

    public void WallJump() {
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * wall.force.y, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * wall.force.x * -birdDirection.lookingDirectionX, ForceMode2D.Impulse);

        wall.timer = 0;
        second.hasJumped = false;

        canApplyDownForce = true;
        canLowerGravity = true; 
        curJump = wall;

        StartCoroutine(TempDisableWalk());
    }

    public void ExertDownForce() {
        rb.linearVelocityY = jumpDownVel;
        canApplyDownForce = false;
        canLowerGravity = false; 
    }

    #endregion

    #region buffers

    // combine bufferfirst and buffer avail? 
    public IEnumerator BufferFirstJump() {
        float elapsedTime = 0;

        while (elapsedTime < firstJumpBuffer) {
            if(birdCollision.isGrounded) {
                FirstJump();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }

    // this is for buffering jumps during dash 
    public IEnumerator BufferAvailableJump() {
        float elapsedTime = 0;

        while (elapsedTime < firstJumpBuffer) {
            StartCoroutine(birdInput.CheckForSpaceUpInput());
            if (birdInput.TryPerformJump()) {
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }

    public IEnumerator TryBufferDownForce() { 
        if (birdDash.isDashing) 
            yield break; 

        float curJumpTimer = curJump.timer; 

        // if player jumps, cancel
        while (curJump != null && curJump.timer < waitUntilDownForceTime) {
            // checks if player's current jump changed at any point using timer
            if(curJump.timer < curJumpTimer)
                yield break; 

            curJumpTimer = curJump.timer;

            yield return null; 
        }

        ExertDownForce(); 
    }

    public IEnumerator TempDisableWalk() {
        birdWalk.disableWalk = true;
        yield return new WaitForSeconds(disableWalkTime);
        birdWalk.disableWalk = false; 
    }

    #endregion

    #region jump tweaks
    public void CalculateToApexTime() {
        if (curJump == null) 
        {
            toApexTime = 0; 
            return;
        }

        toApexTime = rb.linearVelocityY / (Physics2D.gravity.y * originalGravityScale); 
        toApexTime = Math.Abs(toApexTime); 
    }

    public void LowerGravityAtApex() {
        // if other functions need to change gravityscale or have applied a conflicting force
        if (birdDash.isDashing || birdCollision.isTouchingWall) 
            return;

        // to check if this calculation should be done  
        if (curJump == null || birdCollision.isGrounded || rb.linearVelocityY == 0)
            return;

        if (!canLowerGravity)
            rb.gravityScale = originalGravityScale; 

        float toApexTimeThreshold = 0.1f;

        if (toApexTime < toApexTimeThreshold) {
            float fraction = 1 - (toApexTime / toApexTimeThreshold);
            float slowDownGravity = Mathf.Lerp(originalGravityScale, originalGravityScale * 0.6f, fraction);
            rb.gravityScale = slowDownGravity; 
        }
        else {
            rb.gravityScale = originalGravityScale; 
        }
    }

    public void UpdateDownForceState() {
        if (rb.linearVelocityY < 0)
        {
            canApplyDownForce = false;
            return;
        }
    }
    #endregion

    #region variable managers

    void UpdateTimers() {
        first.timer += Time.deltaTime;
        second.timer += Time.deltaTime;
        wall.timer += Time.deltaTime;

        timeSinceOffGround += Time.deltaTime;

        if (birdCollision.isGrounded)
            timeSinceOffGround = 0;

        CalculateToApexTime();
    }

    void InitializeJumpVariables() {
        first.name = "first";
        first.force = new Vector2(0, 15);

        second.name = "second";
        second.force = new Vector2(0, 15);

        wall.name = "wall";
        wall.force = new Vector2(5, 15);

        jumps = new List<Jump> { first, second, wall };

        foreach (var jump in jumps) {
            jump.timer = 0;
            jump.hasJumped = false; 
        }

        originalGravityScale = rb.gravityScale;
    }
    #endregion
}
