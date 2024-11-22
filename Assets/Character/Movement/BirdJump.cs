using UnityEngine;
using System.Collections; // allows for IEnumerator

public class BirdJump : BirdCore
{
    #region forces
    public float jumpForce = 15;
    public float doubleJumpForce = 15;
    public float wallJumpForceX;
    public float wallJumpForceY;
    public float jumpDownVel = -2.5f;
    #endregion

    #region bools
    [HideInInspector] public bool isGrounded = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool hasDoubleJumped = false;
    [HideInInspector] public bool isTouchingWall = false;
    #endregion

    #region timers
    [HideInInspector] public float timeSinceJump;
    [HideInInspector] public float timeSinceWallJump;
    [HideInInspector] public float timeSinceOffGround;
    #endregion

    #region buffers
    [HideInInspector] public float timeToThreeFourthsJumpHeight = 0.5f;
    [HideInInspector] public float coyoteTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    [HideInInspector] public float normalJumpBuffer = 0.2f;
    [HideInInspector] public float timeToThreeFourthUp = 0.45f; // amount of time it takes to reach 3/4ths of apex of the jump. change this value later so it is automatically calculated
    [HideInInspector] public float timeToThreeFourthDown = 0.6f;
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


    void Start()
    {
        levelLayer = LayerMask.GetMask("level");
        originalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        timeSinceJump += Time.deltaTime;
        timeSinceWallJump += Time.deltaTime;
        timeSinceOffGround += Time.deltaTime;
        
        if(isGrounded)
        {
            timeSinceOffGround = 0; 
        }

        CheckFloorCollision();
        CheckWallCollision();

        CheckForSlowDownOnApex();

        ManageJumpInputs();
    }

    public void ManageJumpInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // jump buffer
            if (isJumping && hasDoubleJumped)
            {
                StartCoroutine(BufferNormalJump());
            }

            // wall jump takes priority over double jump
            if ((!isJumping && isGrounded) || (timeSinceOffGround < coyoteTime))
            {
                NormalJump();
            }
            else if (!isGrounded && isTouchingWall)
            {
                WallJump();
            }
            else if (!isGrounded && !hasDoubleJumped && timeSinceJump > .05)
            {
                DoubleJump();
            }
        }

        // if space let go, down force
        if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocityY > 0  && timeSinceJump < timeToThreeFourthUp) 
        {
            ExertDownForce();
        }
    }

    public void NormalJump()
    {
        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        isJumping = true;
        timeSinceJump = 0;
    }

    public void DoubleJump()
    {
        // Debug.Log("double jump");

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
        hasDoubleJumped = true;
    }

    public void WallJump()
    {
        // Debug.Log("wall jump");

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * wallJumpForceY, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * wallJumpForceX * -birdDirection.lookingDirectionX, ForceMode2D.Impulse); 

        timeSinceWallJump = 0;

        StartCoroutine(DisableWalkFromWallJump());
    }

    public void ExertDownForce()
    {
        // Debug.Log("down force");
        rb.linearVelocityY = jumpDownVel;
    }

    public void CheckForSlowDownOnApex()
    {
        if (timeSinceJump > timeToThreeFourthUp && timeSinceJump < timeToThreeFourthDown)
        {
            rb.gravityScale = originalGravityScale * 0.7f;
        }
        else
        {
            rb.gravityScale = originalGravityScale; 
        }
    }

    public IEnumerator BufferNormalJump()
    {
        float elapsedTime = 0;

        while (elapsedTime < normalJumpBuffer)
        {
            if(isGrounded)
            {
                NormalJump();
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // waits until the next frame to continue
        }
    }

    public IEnumerator DisableWalkFromWallJump()
    {
        disableWalk = true;
        yield return new WaitForSeconds(disableWalkTime);
        disableWalk = false; 
    }

    public void CheckFloorCollision()
    {
        if(timeSinceJump < ignoreGroundCheckTime)
        {
            return; 
        }

        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, rayLenY, levelLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayLenY);

        if (downRay)
        {
            isGrounded = true;
            hasDoubleJumped = false;
            isJumping = false;
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
}
