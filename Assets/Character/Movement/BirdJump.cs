using System.Collections.Generic; // for List
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System; // allows for IEnumerator

[System.Serializable]
public class Jump
{
    public string name;
    public Vector2 force;
    public float timer;
    public bool hasJumped;
    public float threeFourthsTimeUp;
    public float threeFourthsTimeDown;
    public float threeFourthsVelocity; 
}

public class BirdJump : BirdCore
{
    public Jump first;
    public Jump second;
    public Jump wall;

    List<Jump> jumps;
    [HideInInspector] public Jump lastJump;

    public float jumpDownVel = -2.5f;

    [HideInInspector] public float timeSinceOffGround;

    #region bools
    public bool isGrounded = false;
    [HideInInspector] public bool isTouchingWall = false;
    [HideInInspector] public bool canApplyDownForce = true;
    #endregion

    #region buffers
    [HideInInspector] public float timeToThreeFourthsJumpHeight = 0.5f;
    [HideInInspector] public float coyoteTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    [HideInInspector] public float firstJumpBuffer = 0.2f;
    [HideInInspector] public float originalGravityScale; 
    #endregion

    #region wall jump manager
    [HideInInspector] public bool disableWalk = false;
    [HideInInspector] public float disableWalkTime = 0.15f;
    #endregion
    

    public float timeScale = 1; 
    void Start()
    {
        InitializeVariables(); 
    }

    void Update()
    {
        Time.timeScale = timeScale; 
        ManageTimers();

        // check if this doesn't break anything. it shouldn't tho. if it doesn't remove CheckForSlowDownOnApex's isdashing check
        // if (birdDash.isDashing) {
        //     return; 
        // }

        CheckForSlowDownOnApex();
        CanDownForceManager();
    }

    // although the same as secondJump, i am keeping these separate in case of needing to make first jump unique
    public void FirstJump()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(first.force, ForceMode2D.Impulse);
        
        first.hasJumped = true;
        first.timer = 0; 

        canApplyDownForce = true;

        lastJump = first;
    }

    public void SecondJump()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(second.force, ForceMode2D.Impulse);

        second.hasJumped = true;
        second.timer = 0;

        canApplyDownForce = true;

        lastJump = second;
    }

    public void WallJump()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * wall.force.y, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * wall.force.x * -birdDirection.lookingDirectionX, ForceMode2D.Impulse);

        wall.timer = 0;
        second.hasJumped = false;

        canApplyDownForce = true;
        lastJump = wall;

        StartCoroutine(DisableWalkFromWallJump());
    }

    public void ExertDownForce()
    {
        rb.linearVelocityY = jumpDownVel;
        canApplyDownForce = false;
    }

    public void CanDownForceManager()
    {
        if(rb.linearVelocityY < 0)
        {
            canApplyDownForce = false;
            return;
        }
        
        if (lastJump == null)
        {
            return; 
        }

        if (lastJump.timer > lastJump.threeFourthsTimeUp)
        {
            canApplyDownForce = false; 
        }
    }

    public void CheckForSlowDownOnApex()
    {
        if (birdDash.isDashing)
        {
            return;
        }

        if (lastJump == null || isGrounded || rb.linearVelocityY == 0)
        {
            return;
        }


        // possible ways to implement: 
        // timer: during a certain timeframe after the player's jump, slowdown  
        // velocity: during a certain vel threshold, slowdown 
            // calculated by vi / 2 to reach 3/4ths height 
        // accel + vel: using current accel and vel to calculate the amount of TIME needed to reach the top. call this toApexTime 
        // when toApexTime reaches a certain threshold, slowdown 
        // this should work on both the way up and down by absolute val velocity

        // toApexTime calculation: 
        // initial vel = current vel
        // final vel = 0 
        // vi = vo + at
        // vi = 0 + at
        // t = vi / a 

        float toApexTime = rb.linearVelocityY / (Physics2D.gravity.y * rb.gravityScale);
        toApexTime = Math.Abs(toApexTime); 

        Debug.Log(toApexTime);

        // TODO: make it a gradual slowdown rather than immediate
        if (toApexTime < .2f) {
            // slowdown
            rb.gravityScale = originalGravityScale * 0.7f;
        }
        else
        {
            rb.gravityScale = originalGravityScale; 
        }

    }

    public IEnumerator BufferFirstJump()
    {
        float elapsedTime = 0;

        while (elapsedTime < firstJumpBuffer)
        {
            if(isGrounded)
            {
                FirstJump();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // waits until the next frame to continue
        }
    }

    public IEnumerator DisableWalkFromWallJump()
    {
        birdWalk.disableWalk = true;
        yield return new WaitForSeconds(disableWalkTime);
        birdWalk.disableWalk = false; 
    }

    void ManageTimers()
    {
        first.timer += Time.deltaTime;
        second.timer += Time.deltaTime;
        wall.timer += Time.deltaTime;

        timeSinceOffGround += Time.deltaTime;

        if (isGrounded)
        {
            timeSinceOffGround = 0;
        }
    }

    void InitializeVariables()
    {
        first.name = "first";
        first.force = new Vector2(0, 15);
        first.threeFourthsTimeUp = 0.45f;
        first.threeFourthsTimeDown = 0.60f;
        first.threeFourthsVelocity = first.force.y / 2; // solved using phys, add to documentation later

        second.name = "second";
        second.force = new Vector2(0, 15);
        second.threeFourthsTimeUp = 0.45f;
        second.threeFourthsTimeDown = 0.60f;

        wall.name = "wall";
        wall.force = new Vector2(5, 15);
        wall.threeFourthsTimeUp = 0.45f;
        wall.threeFourthsTimeDown = 0.60f;

        jumps = new List<Jump> { first, second, wall };

        foreach (var jump in jumps) {
            jump.timer = 0;
            jump.hasJumped = false; 
            jump.threeFourthsVelocity = jump.force.y / 2;
        }

        originalGravityScale = rb.gravityScale;
    }
}
