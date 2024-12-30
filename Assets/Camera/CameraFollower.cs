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

    void Start() {
        
    }

    void LateUpdate() {
        if (playerTransform == null) 
            return;

        // run through AddLookahead to calculate targetPosition 
        Transform targetPosition = AddLookahead(playerTransform); 

        // run through SmoothCameraFollow to smoothly move 
        SmoothCameraFollow(targetPosition); 
    }

    public void SmoothCameraFollow(Transform targetPosition) { 
        Vector3 targetPos = targetPosition.position;
        targetPos.z = transform.position.z; 

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothFactor);
    }

    // takes the player's position and returns it with the lookahead added to it
    public Transform AddLookahead(Transform playerTransform) {
        Vector3 targetposition = playerTransform.position; 
        

        return playerTransform;
    }
}
