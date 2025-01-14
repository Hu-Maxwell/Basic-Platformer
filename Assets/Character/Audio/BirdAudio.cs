using UnityEngine;
using UnityEngine.Audio;

// this kinda sucks cuz steps play even if in air, also same sound effect 

public class BirdAudio : BirdCore {

    private AudioSource audioSource;
    public AudioClip stepSound;
    
    private float timeSinceLastStep; 
    public float timeBetweenStep;

    void Start() {
        GameObject birdGameObject = GameObject.Find("bird");
        rb = birdGameObject.GetComponent<Rigidbody2D>(); 

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null) 
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = stepSound;
    }

    void Update() {
        timeSinceLastStep += Time.deltaTime; 

        PlayStep();
    }

    private void PlayStep() {
        // other option: wait until sound effect is finished 
            // !audioSource.isPlaying

        if (Mathf.Abs(rb.linearVelocityX) > 0.2f && timeSinceLastStep > timeBetweenStep) {
            audioSource.Play();
            timeSinceLastStep = 0; 
        }
    }

}
