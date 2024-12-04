using System.Collections.Generic; // for List
using UnityEngine;
using System.Collections; // allows for IEnumerator

[System.Serializable]
public class Jump
{
    public string name;
    public Vector2 force;
    public float timer;
    public bool hasJumped;
    public float threeFourthsTimeUp;
    public float threeFourthsTimeDown;
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
    [HideInInspector] public bool isGrounded = false;
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
    
    void Start()
    {
        InitializeVariables(); 
    }

    void Update()
    {
        ManageTimers();

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

    // TODO: add bounciness based off of vel
    // phys eq: vel @ 3/4 = vi / 2 
    public void CheckForSlowDownOnApex()
    {
        if (birdDash.isDashing)
        {
            return;
        }

        if (lastJump == null)
        {
            return;
        }

        // change to variable, if it's between +2 and -2 vel
        if (lastJump.timer > lastJump.threeFourthsTimeUp && lastJump.timer < lastJump.threeFourthsTimeDown) 
        {
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
        first.timer = 0;
        first.hasJumped = false;
        first.threeFourthsTimeUp = 0.45f;
        first.threeFourthsTimeDown = 0.60f;

        second.name = "second";
        second.force = new Vector2(0, 15);
        second.timer = 0;
        second.hasJumped = false;
        second.threeFourthsTimeUp = 0.45f;
        second.threeFourthsTimeDown = 0.60f;

        wall.name = "wall";
        wall.force = new Vector2(5, 15);
        wall.timer = 0;
        wall.hasJumped = false;
        wall.threeFourthsTimeUp = 0.45f;
        wall.threeFourthsTimeDown = 0.60f;

        jumps = new List<Jump> { first, second, wall };
        originalGravityScale = rb.gravityScale;
    }
}
