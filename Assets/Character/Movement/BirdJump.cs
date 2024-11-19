using UnityEngine;
using System.Collections; // allows for IEnumerator

public class BirdJump : BirdCore
{
    #region vars
    // bools
    public bool isGrounded = false;
    public bool isJumping = false;
    public bool hasDoubleJumped = false;
    public bool isTouchingWall = false;

    // timers
    public float timeSinceJump;
    // public float timeSinceDoubleJump
    public float timeSinceWallJump;
    public float timeSinceOffGround; 

    // first jump
    public float jumpForce = 15;

    // double jump
    public float doubleJumpForce = 15;

    // wall jump
    public float wallJumpForceX;
    public float wallJumpForceY;

    // down force
    public float jumpDownVel = -2.5f;

    // buffers
    [HideInInspector] public float timeToThreeFourthsJumpHeight = 0.5f; // replace this with an equation later
    public float coyoteTime = 0.1f;
    [HideInInspector] public float wallJumpBufferX;
    [HideInInspector] public float normalJumpBuffer = 0.2f;

    // collisions
    [HideInInspector] public LayerMask levelLayer;
    [HideInInspector] public float rayLenX = .525f;
    [HideInInspector] public float rayLenY = 1.01f;
    public float ignoreGroundCheckTime = 0.05f;

    #endregion

    void Start()
    {
        levelLayer = LayerMask.GetMask("level");
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

            if ((!isJumping && isGrounded) || (timeSinceOffGround < coyoteTime))
            {
                NormalJump();
            }
            else if (!isGrounded && !hasDoubleJumped && timeSinceJump > .05)
            {
                DoubleJump();
            }
        }

        // if space let go, down force
        if (Input.GetKeyUp(KeyCode.Space)) // && !hasDoubleJumped && rb.linearVelocityY > 0 && timeSinceJump < timeToThreeFourthsJumpHeight
        {
            ExertDownForce();
        }
    }

    public void NormalJump()
    {
        Debug.Log("jump");

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

    /*
    public IEnumerator WallJump()
    {
        yield return new WaitForSeconds(0.05f);

        if (Input.GetKeyDown(KeyCode.Space) && isTouchingWall)
        {
            timeSinceWallJump = 0;
            Debug.Log("wall jump");
            rb.linearVelocityY = 0;
            rb.AddForce(Vector2.up * wallJumpForceY, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * wallJumpForceX * -lookingDirection.x, ForceMode2D.Impulse);
        }
    }
    */

    public void ExertDownForce()
    {
        // Debug.Log("down force");
        rb.linearVelocityY = jumpDownVel;
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
