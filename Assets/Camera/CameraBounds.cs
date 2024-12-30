using System;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// makes camera follow the character around
/// </summary>
public class CameraBounds : CameraCore
{
    public Bounds bounds; 

    void Start() {
        bounds = cameraArea.bounds; 


    }

    // get camera bounds 
    void Update() { 
        if (transform.position.x < bounds.min.x || 
        transform.position.x > bounds.max.x) {  
            Debug.Log("camera out of bounds X"); 
        }

        if (transform.position.y < bounds.min.y || 
        transform.position.y > bounds.max.y) {  
            Debug.Log("camera out of bounds Y"); 
        }
    }

}
