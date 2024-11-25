using UnityEngine;

public class BirdCore : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rb;

    [HideInInspector] public BirdWalk birdWalk;
    [HideInInspector] public BirdJump birdJump;
    [HideInInspector] public BirdDash birdDash;
    [HideInInspector] public BirdDirection birdDirection; 

    void Awake()
    {
        birdWalk = GetComponent<BirdWalk>();
        birdJump = GetComponent<BirdJump>();
        birdDash = GetComponent<BirdDash>();
        birdDirection = GetComponent<BirdDirection>();

        rb = GetComponent<Rigidbody2D>();
    }
}
