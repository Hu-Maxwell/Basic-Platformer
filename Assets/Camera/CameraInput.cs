using System.Linq;
using UnityEngine;

public class CameraInput : CameraCore {
    
    /// <summary>
    /// moves the camera around based off of user input
    /// </summary>
    
    [HideInInspector] public float lookKeyHeldTimer;
    public float cameraLookahead; 

    void Start() {
        
    }

    void Update() {
        
    }

    public float CheckInputs() { 
        if (Input.GetKey(KeyCode.W)) {
            lookKeyHeldTimer+= Time.deltaTime;

            if (lookKeyHeldTimer >= 2) 
                return cameraLookahead; 
            else 
                return 0; 

        } 
        else if (Input.GetKey(KeyCode.S)) {
            // TODO
            return 0;
        }
        else 
        {
            return 0; 
        }
    }
}
