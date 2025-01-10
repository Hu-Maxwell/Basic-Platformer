 using Unity.Collections;
using UnityEngine;

/// <summary>
/// initializes everything the camera scripts need
/// </summary>

public class CameraCore : MonoBehaviour {

    public BirdCollision birdCollision;
    public BirdDirection birdDirection; 
    public Collider2D cameraArea; 

    public CameraFollower cameraFollower;
    public CameraInput cameraInput; 

    void Start() {
    }

    void Update() {

    }
}
