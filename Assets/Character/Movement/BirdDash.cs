using UnityEngine;
using System.Collections; // allows for IEnumerator

public class BirdDash : BirdCore
{
    #region vars
    [HideInInspector] public bool isDashing;
    [HideInInspector] public float timeSinceDashEnd;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool firstDash = true; 
    [HideInInspector] float originalGravityScale;

    public float dashMultiplier = 3;
    public float dashTime = .25f;
    float dashCooldown = 0.3f;
    #endregion

    public void Update()
    {
        originalGravityScale = rb.gravityScale;
        timeSinceDashEnd += Time.deltaTime;
        CanDashManager();
    }

    public IEnumerator Dash()
    {
        birdWalk.disableWalk = true;
        isDashing = true;

        float direction = birdDirection.lookingDirectionX; 

        if (birdJump.isTouchingWall) {
            direction = -direction; 
        }
        float oldVelX = rb.linearVelocityX;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(birdWalk.moveSpeed * dashMultiplier * direction, 0);
        
        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravityScale;

        // prevents the bird moving the opposite side after dash ends
        if (Mathf.Sign(oldVelX) != birdDirection.lookingDirectionX)
        {
            rb.linearVelocityX = 0; 
        }
        else
        {
            rb.linearVelocityX = oldVelX;
        }


        timeSinceDashEnd = 0;
        birdWalk.disableWalk = false; 
        isDashing = false;
    }

    public void CanDashManager()
    {
        // allows for first dash the first dashCooldown seconds of program
        if (firstDash)
        {
            if (isDashing)
            {
                firstDash = false;
            }
            return; 
        }

        canDash = timeSinceDashEnd > dashCooldown;
    }
}