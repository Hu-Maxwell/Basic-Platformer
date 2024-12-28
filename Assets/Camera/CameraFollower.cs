using UnityEngine;

public class CameraFollower : CameraCore
{
    /// <summary>
    /// makes camera follow the character around
    /// </summary>
    /// 

    // take bird's transform (for camera follower)
    public Transform targetTransform; 
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start() {
        
    }

    void LateUpdate() {
        if (targetTransform == null) {
            return;
        }
        Vector3 currentPosition = transform.position;

        Vector3 targetPosition = new Vector3(
            targetTransform.position.x + offset.x,
            Mathf.Max(targetTransform.position.y + offset.y, -1f), 
            currentPosition.z
        );

        // Smoothly interpolate between the current position and the target position
        transform.position = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed);
    }
}
