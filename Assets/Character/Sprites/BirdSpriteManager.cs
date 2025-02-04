using UnityEngine;

// UNUSED
public class BirdSpriteManager : BirdCore {
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;

    public Sprite baseSprite;
    public Sprite walkingSprite;

    void Start() {
        // since the sprite manager script is not attached to the bird game object, it can't access the same stuff from birdCore
        GameObject birdGameObject = GameObject.Find("bird");
        birdDirection = birdGameObject.GetComponent<BirdDirection>();
        birdDash = birdGameObject.GetComponent<BirdDash>();

        rb = birdGameObject.GetComponent<Rigidbody2D>(); 

        spriteRenderer.sprite = baseSprite;
    }

    void Update() {
        UpdateBirdSpriteDirection();

        UpdateBirdSpriteWalk();

        TrailManager();
    }

    public void UpdateBirdSpriteDirection() {
        if (birdDirection.lookingDirectionX == -1)
            spriteRenderer.flipX = false;
        if (birdDirection.lookingDirectionX == 1)
            spriteRenderer.flipX = true;
    }

    public void UpdateBirdSpriteWalk() {

        if (Mathf.Abs(rb.linearVelocityX) > 0.2f)
            spriteRenderer.sprite = walkingSprite;
        
        else
            spriteRenderer.sprite = baseSprite;
    }

    public void TrailManager() {

        if(birdDash.isDashing) 
            trailRenderer.emitting = true;
        else
            trailRenderer.emitting = false;
    }
}
