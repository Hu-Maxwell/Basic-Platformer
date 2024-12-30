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

    void Start() {
        
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

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
    }

    // takes the player's position and returns it with the lookahead added to it
    public Vector3 AddLookahead(Transform playerTransform) {
        Vector3 targetposition = playerTransform.position; 

        targetposition.x += lookaheadDistance * birdDirection.lookingDirectionX; 

        return targetposition;
    }
}
