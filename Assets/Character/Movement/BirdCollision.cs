using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;

public class BirdCollision : BirdCore
{
    [HideInInspector] public LayerMask levelLayer;
    [HideInInspector] public float rayLenX = 0.525f;
    public float rayLenY = 1.005f;
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

    void CheckWallCollision()
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
}