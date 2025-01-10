using System.Linq;
using UnityEngine;

public class CameraInput : CameraCore {
    
    /// <summary>
    /// moves the camera around based off of user input
    /// </summary>
    
    [HideInInspector] public float heldTimerW;
    [HideInInspector] public float heldTimerS;
    public float cameraLookahead; 

    void Start() {
        
    }

    void Update() {
        CheckInputs();
    }

    public void CheckInputs() { 
        if (Input.GetKey(KeyCode.W))
            heldTimerW += Time.deltaTime;
        else 
            heldTimerW = 0;

        if (Input.GetKey(KeyCode.S))
            heldTimerS += Time.deltaTime; 
        else 
            heldTimerS = 0;

    }

    public float AddLookaheadY() {
        if (heldTimerW >= 1 || heldTimerS >= 1) {
            
            if (heldTimerW >= heldTimerS)
                return cameraLookahead; 
            else 
                return -cameraLookahead; 
        }

        return 0;
    }
}
