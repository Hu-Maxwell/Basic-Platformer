using UnityEngine;
using System.Collections; // allows for IEnumerator

public class BirdDash : BirdCore
{
    #region vars
    public bool isDashing;
    public float timeSinceDashEnd;
    public bool canDash = true;
    public bool firstDash = true; 

    public float dashMultiplier = 3;
    public float dashTime = 0.5f;
    float dashCooldown = 0.3f;
    #endregion

    public void Update()
    {
        timeSinceDashEnd += Time.deltaTime;
        CanDashManager(); 

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Dash()
    {
        birdWalk.disableWalk = true;
        isDashing = true;

        float oldVelX = rb.linearVelocityX;
        float originalGravity = rb.gravityScale;

        rb.gravityScale = 0;
        rb.linearVelocity = new Vector2(birdWalk.moveSpeed * dashMultiplier * birdDirection.lookingDirectionX, 0);
        
        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;

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