using UnityEngine;
using System.Collections; // allows for IEnumerator

/// <summary>
/// executes the bird's dash movement by applying horizontal velocity and disabling gravity
/// </summary>
public class BirdDash : BirdCore {
    #region vars
    [HideInInspector] public bool isDashing;
    [HideInInspector] public float timeSinceDashEnd;
    [HideInInspector] public bool firstDash = true; 
    [HideInInspector] public float originalGravityScale;

    [SerializeField] private float dashMultiplier = 3;
    [SerializeField] private float dashTime = .25f;
    [SerializeField] public float dashCooldown = 0.3f;
    
    #endregion

    public void Start() {
        originalGravityScale = rb.gravityScale;
    }

    public void Update() {
        timeSinceDashEnd += Time.deltaTime;
    }

    private void ApplyDashForce(float direction) {
        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(birdWalk.MoveSpeed * dashMultiplier * direction, 0);
    }

    private void RestorePostDash(float oldVelX) {
        rb.gravityScale = originalGravityScale;

        // prevents bird from looking in opposite dir after dash ends
        if (Mathf.Sign(oldVelX) != birdDirection.lookingDirectionX)
            rb.linearVelocityX = 0; 
        else
            rb.linearVelocityX = oldVelX;

        timeSinceDashEnd = 0;
        birdWalk.disableWalk = false;
        isDashing = false;
    }

    private float GetDashDirection() {
        // reverses direction if bird is touching wall
        return birdJump.isTouchingWall 
            ? -birdDirection.lookingDirectionX 
            : birdDirection.lookingDirectionX;
    }

    public IEnumerator Dash() {
        birdWalk.disableWalk = true;
        isDashing = true;
        firstDash = false; 

        float direction = GetDashDirection();
        float oldVelX = rb.linearVelocityX;

        ApplyDashForce(direction);

        yield return new WaitForSeconds(dashTime);

        RestorePostDash(oldVelX);
    }
}