using System.Collections.Generic; 
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering;

[System.Serializable]
public class Jump {
    public string name;
    public Vector2 force;
    public float timer;
    public bool hasJumped;
}

public class BirdJump : BirdCore {
    public Jump first;
    public Jump second;
    public Jump wall;

    List<Jump> jumps;
    [HideInInspector] public Jump curJump;

    [HideInInspector] public float jumpDownVel = -2.0f;
    [HideInInspector] public float toApexTime = 0; 
    [HideInInspector] public float timeSinceOffGround;

    #region bools
    [HideInInspector] public bool canApplyDownForce = true;
    #endregion

    #region buffers
    [HideInInspector] public float coyoteTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    [HideInInspector] public float firstJumpBuffer = 0.03f;
    [HideInInspector] public float originalGravityScale; 
    #endregion

    #region wall jump manager
    [HideInInspector] public bool disableWalk = false;
    [HideInInspector] public float disableWalkTime = 0.15f;
    #endregion
    
    void Start() {
        InitializeVariables(); 
    }

    void Update() {
        ManageTimers();

        ApexSlowDownManager();
        CanDownForceManager();
    }

    // keeping FirstJump and SecondJump separate in case if needed
    public void FirstJump() {
        rb.linearVelocityY = 0;
        rb.AddForce(first.force, ForceMode2D.Impulse);
        
        first.hasJumped = true;
        first.timer = 0; 

        canApplyDownForce = true;

        curJump = first;
    }

    public void SecondJump() {
        rb.linearVelocityY = 0;
        rb.AddForce(second.force, ForceMode2D.Impulse);

        second.hasJumped = true;
        second.timer = 0;

        canApplyDownForce = true;

        curJump = second;
    }

    public void WallJump() {
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * wall.force.y, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * wall.force.x * -birdDirection.lookingDirectionX, ForceMode2D.Impulse);

        wall.timer = 0;
        second.hasJumped = false;

        canApplyDownForce = true;
        curJump = wall;

        StartCoroutine(DisableWalkFromWallJump());
    }

    public void ExertDownForce() {
        rb.linearVelocityY = jumpDownVel;
        canApplyDownForce = false;
    }

    public void CalculateToApexTime() {
        if (curJump == null) 
        {
            toApexTime = 0; 
            return;
        }

        toApexTime = rb.linearVelocityY / (Physics2D.gravity.y * originalGravityScale); 
        toApexTime = Math.Abs(toApexTime); 
    }

    public void CanDownForceManager() {
        if (rb.linearVelocityY < 0)
        {
            canApplyDownForce = false;
            return;
        }
        
        if (curJump == null)
        {
            return; 
        }

        float toApexTimeThreshold = 0.2f; // this line is repeated in ApexSlowDownManager 
        if (toApexTime < toApexTimeThreshold) 
        {
            canApplyDownForce = false; 
        }
    }

    public void ApexSlowDownManager() {
        // prolly combine these two or smth
        if (birdDash.isDashing || birdCollision.isTouchingWall) 
        {
            return;
        }

        if (curJump == null || birdCollision.isGrounded || rb.linearVelocityY == 0)
        {
            return;
        }



        float toApexTimeThreshold = 0.2f;

        if (toApexTime < toApexTimeThreshold) {
            float fraction = 1 - (toApexTime / toApexTimeThreshold);
            float slowDownGravity = Mathf.Lerp(originalGravityScale, originalGravityScale * 0.6f, fraction);
            rb.gravityScale = slowDownGravity; 
        }
        else {
            rb.gravityScale = originalGravityScale; 
        }
    }

    public IEnumerator BufferFirstJump() {
        float elapsedTime = 0;

        while (elapsedTime < firstJumpBuffer) {
            if(birdCollision.isGrounded) {
                FirstJump();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // waits until the next frame to continue
        }
    }

    public IEnumerator BufferAnyJump() {
        float elapsedTime = 0;

        while (elapsedTime < firstJumpBuffer) {
            if (birdInput.TryPerformJump()) 
                yield break;

            elapsedTime += Time.deltaTime;
            yield return null; 
        }
    }

    public IEnumerator BufferDownForce() { 
        // while time since jump is < .1
        float curJumpTimer = curJump.timer; 
        while (curJump != null && curJump.timer < 0.1f) {
            // if player jumps, cancel
            // checks if player's current jump changed at any point using timer
            if(curJump.timer < curJumpTimer)
                yield break; 

            curJumpTimer = curJump.timer;

            yield return null; 
        }

        ExertDownForce(); 
    }

    public void TryBufferDownForce() {
        if (curJump != null && curJump.timer < 0.1f) // kinda bad bc .1f is repeated twice in the code. try to get a variable for this
            StartCoroutine(BufferDownForce());
        else
            ExertDownForce(); 
    }


    public IEnumerator DisableWalkFromWallJump() {
        birdWalk.disableWalk = true;
        yield return new WaitForSeconds(disableWalkTime);
        birdWalk.disableWalk = false; 
    }

    void ManageTimers() {
        first.timer += Time.deltaTime;
        second.timer += Time.deltaTime;
        wall.timer += Time.deltaTime;

        timeSinceOffGround += Time.deltaTime;

        if (birdCollision.isGrounded)
            timeSinceOffGround = 0;

        CalculateToApexTime();
    }

    void InitializeVariables() {
        first.name = "first";
        first.force = new Vector2(0, 15);

        second.name = "second";
        second.force = new Vector2(0, 15);

        wall.name = "wall";
        wall.force = new Vector2(5, 15);

        foreach (var jump in jumps) {
            jump.timer = 0;
            jump.hasJumped = false; 
        }

        originalGravityScale = rb.gravityScale;
    }
}
