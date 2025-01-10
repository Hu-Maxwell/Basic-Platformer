using System;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// makes camera follow the character around
/// </summary>
public class CameraFollower : CameraCore
{
    public Transform playerTransform; // bird's transform

    public float smoothFactor;
    public float lookaheadDistance; 
    public Bounds bounds; 

    void Start() {
        bounds = cameraArea.bounds; 
    }

    void LateUpdate() {
        if (playerTransform == null) 
            return;

        // run through AddLookahead to calculate targetPosition 
        Vector3 targetPosition = AddLookahead(playerTransform); 

        // run through SmoothCameraFollow to smoothly move 
        SmoothCameraFollow(targetPosition); 
    }

    public void SmoothCameraFollow(Vector3 targetPosition) { 
        targetPosition.z = transform.position.z; 

        targetPosition = CheckBounds(targetPosition); 

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
    }

    public Vector3 CheckBounds(Vector3 targetPosition) {
        targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);

        return targetPosition;
    }

    // takes the player's position and returns it with the lookahead added to it
    public Vector3 AddLookahead(Transform playerTransform) {
        Vector3 targetposition = playerTransform.position; 

        targetposition.x += lookaheadDistance * birdDirection.lookingDirectionX; 

        // if input pressed 
        targetposition.y += cameraInput.CheckInputs(); 

        return targetposition;
    }
}
