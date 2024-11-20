using UnityEngine;

public class BirdWalk : BirdCore
{
    public float moveSpeed = 5;
    public float accelAmount = 10;
    public float decelAmount = 3;
    public float velPower = 0.9f;

    void FixedUpdate()
    {
        birdWalk.Walk(Input.GetAxisRaw("Horizontal"));
    }

    public void Walk(float direction)
    {
        float targetSpeed = direction * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? accelAmount : decelAmount;

        float forceApplied = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(forceApplied * Vector2.right);
    }
}