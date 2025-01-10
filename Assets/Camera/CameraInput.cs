using System.Linq;
using UnityEngine;

public class CameraInput : CameraCore {
    
    /// <summary>
    /// moves the camera around based off of user input
    /// </summary>
    
    public float lookKeyHeldTimer; 


    void Start() {
        
    }

    void Update() {
        CheckInputs();
    }

    public void CheckInputs() { 
        if (Input.GetKey(KeyCode.W)) {
            lookKeyHeldTimer+= Time.deltaTime;
            if (lookKeyHeldTimer >= 2) {
                ShiftCameraY(); 
            }
        } else {
            lookKeyHeldTimer = 0; 
        }
    }

    // takes as a parameter whether to look up or down, then looks wherever
    void float ShiftCameraY() {
        return 
    }
}
