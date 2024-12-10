using UnityEngine;

public class BirdWalk : BirdCore
{
    #region vars
    [HideInInspector] public bool disableWalk = false;

    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float accelAmount = 10;
    [SerializeField] private float decelAmount = 3;
    [SerializeField] private float velPower = 0.9f;

    private const float MIN_SPEED_THRESHOLD = 0.01f;
    #endregion

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    private float CalculateAccelerationRate(float targetSpeed)
    {
        return (Mathf.Abs(targetSpeed) > MIN_SPEED_THRESHOLD) ? accelAmount : decelAmount;
    }

    private float CalculateForce(float speedDiff, float accelRate)
    {
        return Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);
    }

    public void Walk(float direction)
    {
        if (disableWalk) return;

        float targetSpeed = direction * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = CalculateAccelerationRate(targetSpeed);
        float forceApplied = CalculateForce(speedDiff, accelRate);

        rb.AddForce(new Vector2(forceApplied, 0));
    }

}