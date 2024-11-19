using UnityEngine;

public class BirdCore : MonoBehaviour
{
    public Rigidbody2D rb;

    [HideInInspector] public BirdWalk birdWalk;
    [HideInInspector] public BirdJump birdJump;
    [HideInInspector] public BirdDash birdDash;

    void Start()
    {
        birdWalk = GetComponent<BirdWalk>();
        birdJump = GetComponent<BirdJump>();
        birdDash = GetComponent<BirdDash>();
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
