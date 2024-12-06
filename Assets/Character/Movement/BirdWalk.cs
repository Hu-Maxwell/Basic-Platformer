using UnityEngine;

public class BirdWalk : BirdCore
{
    #region vars
    [HideInInspector] public bool disableWalk = false;

    public float moveSpeed = 5;
    public float accelAmount = 10;
    public float decelAmount = 3;
    public float velPower = 0.9f;
    #endregion

    public void Walk(float direction)
    {
        float targetSpeed = direction * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? accelAmount : decelAmount;

        float forceApplied = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(new Vector2(forceApplied, 0));
    }
}