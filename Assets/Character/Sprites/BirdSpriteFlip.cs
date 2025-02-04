using UnityEngine;

public class BirdSpriteFlip : MonoBehaviour {
    // flips the bird's sprite depending on direction moving

    public BirdDirection birdDirection; 
    public SpriteRenderer spriteRenderer;
    
    void Start() {
        GameObject birdGameObject = GameObject.Find("bird");
        birdDirection = birdGameObject.GetComponent<BirdDirection>();
    }

    void Update() {
        UpdateBirdSpriteDirection();
    }

    public void UpdateBirdSpriteDirection() {
        if (birdDirection.lookingDirectionX == 1)
            spriteRenderer.flipX = false;
        else if (birdDirection.lookingDirectionX == -1)
            spriteRenderer.flipX = true;
    }
}
