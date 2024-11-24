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

    #region forces
    public float jumpDownVel = -2.5f;
    #endregion

    #region bools
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isTouchingWall = false;
    [HideInInspector] public bool canApplyDownForce = true;
    #endregion

    #region timers
    [HideInInspector] public float timeSinceOffGround;
    #endregion

    #region buffers
    [HideInInspector] public float timeToThreeFourthsJumpHeight = 0.5f;
    [HideInInspector] public float coyoteTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    [HideInInspector] public float firstJumpBuffer = 0.2f;
    [HideInInspector] float originalGravityScale; 
    #endregion

    #region wall jump timer
    [HideInInspector] public bool disableWalk = false;
    [HideInInspector] public float disableWalkTime = 0.15f;
    #endregion

    #region collisions
    [HideInInspector] public LayerMask levelLayer;
    [HideInInspector] public float rayLenX = 0.525f;
    [HideInInspector] public float rayLenY = 1.01f;
    [HideInInspector] public float ignoreGroundCheckTime = 0.05f; 
    #endregion
    
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
    }

    void Start()
    {
        levelLayer = LayerMask.GetMask("level");
        originalGravityScale = rb.gravityScale;
        InitializeVariables(); 
    }

    void Update()
    {
        ManageTimers();
        InputManager();

        CheckFloorCollision();
        CheckWallCollision();

        CheckForSlowDownOnApex();
        CanDownForceManager();
    }

    public void InputManager()
    {
        if (birdDash.isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump buffer
            if (first.hasJumped && second.hasJumped)
            {
                StartCoroutine(BufferFirstJump());
            }

            // wall jump takes priority over second jump
            if ((!first.hasJumped && isGrounded) || (timeSinceOffGround < coyoteTime))
            {
                FirstJump();
            }
            else if (!isGrounded && isTouchingWall)
            {
                WallJump();
            }
            else if (!isGrounded && !second.hasJumped && first.timer > .05)
            {
                SecondJump();
            }
        }

        // if space let go, down force
        if (Input.GetKeyUp(KeyCode.Space) && canApplyDownForce) 
        {
            ExertDownForce();
        }
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

        if (lastJump == null)
        {
            return;
        }

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

    public void CheckFloorCollision()
    {
        if(first.timer < ignoreGroundCheckTime)
        {
            return; 
        }

        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, rayLenY, levelLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayLenY);

        if (downRay)
        {
            isGrounded = true;
            lastJump = null;

            first.hasJumped = false;
            second.hasJumped = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CheckWallCollision()
    {
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, rayLenX, levelLayer);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, rayLenX, levelLayer);

        Debug.DrawRay(transform.position, Vector2.left * rayLenX);
        Debug.DrawRay(transform.position, Vector2.right * rayLenX);

        if (leftRay || rightRay)
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
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
}
