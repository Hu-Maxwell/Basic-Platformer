using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;

public class BirdCollision : BirdCore
{
    [HideInInspector] public LayerMask levelLayer;
    [HideInInspector] public float rayLenX = 0.525f;
    [HideInInspector] public float rayLenY = 1.01f;
    [HideInInspector] public float ignoreGroundCheckTime = 0.05f;

    void Start()
    {
        levelLayer = LayerMask.GetMask("level");
    }

    void Update()
    {
        CheckFloorCollision();
        CheckWallCollision();
    }

    public void CheckFloorCollision()
    {
        if (birdJump.first.timer < ignoreGroundCheckTime)
        {
            return;
        }

        RaycastHit2D downRay = Physics2D.Raycast(transform.position, Vector2.down, rayLenY, levelLayer);
        Debug.DrawRay(transform.position, Vector2.down * rayLenY);

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

    void CheckWallCollision()
    {
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, Vector2.left, rayLenX, levelLayer);
        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, Vector2.right, rayLenX, levelLayer);

        Debug.DrawRay(transform.position, Vector2.left * rayLenX);
        Debug.DrawRay(transform.position, Vector2.right * rayLenX);

        if (leftRay || rightRay)
        {
            birdJump.isTouchingWall = true;
        }
        else
        {
            birdJump.isTouchingWall = false;
        }
    }
}