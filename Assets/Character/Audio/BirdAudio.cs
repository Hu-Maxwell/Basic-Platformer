using UnityEngine;
using UnityEngine.Audio;

public class BirdAudio : BirdCore {
    void Start() {
        
    }

    void Update() {
        PlayStep();
    }

    private void PlayStep() {
        // if bird is moving fast enough, play step sound
        if (Mathf.Abs(rb.linearVelocityX) > 0.2f) {
            // play step sound
        }
    }
}
