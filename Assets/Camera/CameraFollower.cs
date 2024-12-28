using Unity.VisualScripting;
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

    public float cameraLookUp = .8f; 
    public float cameraY = 0; 

    public float dampingSpeed = 0.05f; 
    private float velocityY = 0f; 

    void Start() {
        
    }

    void LateUpdate() {
        if (targetTransform == null) 
            return;
        

        if (birdCollision.isGrounded) {
            cameraY = Mathf.SmoothDamp(cameraY, Mathf.Round(targetTransform.position.y), ref velocityY, dampingSpeed);
        }
        
        Vector3 currentPosition = transform.position;

        Vector3 targetPosition = new Vector3(
            targetTransform.position.x,
            cameraY, 
            currentPosition.z
        );

        transform.position = Vector3.Lerp(currentPosition, targetPosition, smoothSpeed);
    }
}
