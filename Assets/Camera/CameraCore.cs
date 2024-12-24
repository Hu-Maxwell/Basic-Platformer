using Unity.Collections;
using UnityEngine;

/// <summary>
/// initializes everything the camera scripts need
/// </summary>

public class CameraCore : MonoBehaviour {

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
        Vector3 targetPosition = targetTransform.position; 
        transform.position = new Vector3(targetPosition.x, targetPosition.y, currentPosition.z);
    }
}
