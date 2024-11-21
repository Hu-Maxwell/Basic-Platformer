using UnityEngine;

public class BirdCore : MonoBehaviour
{
    public Rigidbody2D rb;

    public BirdWalk birdWalk;
    public BirdJump birdJump;
    public BirdDash birdDash;
    public BirdDirection birdDirection; 

    void Awake()
    {
        birdWalk = GetComponent<BirdWalk>();
        birdJump = GetComponent<BirdJump>();
        birdDash = GetComponent<BirdDash>();
        birdDirection = GetComponent<BirdDirection>();

        rb = GetComponent<Rigidbody2D>();

    }

}
