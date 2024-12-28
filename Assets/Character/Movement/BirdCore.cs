using UnityEngine;

public class BirdCore : MonoBehaviour {
    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public BirdInput birdInput;
    [HideInInspector] public BirdWalk birdWalk;
    [HideInInspector] public BirdJump birdJump;
    [HideInInspector] public BirdDash birdDash;
    [HideInInspector] public BirdDirection birdDirection; 
    public BirdCollision birdCollision;

    void Awake() {
        birdInput = GetComponent<BirdInput>(); 
        birdWalk = GetComponent<BirdWalk>();
        birdJump = GetComponent<BirdJump>();
        birdDash = GetComponent<BirdDash>();
        birdDirection = GetComponent<BirdDirection>();
        birdCollision = GetComponent<BirdCollision>(); 

        rb = GetComponent<Rigidbody2D>();
    }
}
