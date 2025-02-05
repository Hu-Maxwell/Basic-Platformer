using UnityEngine;

public class BirdDashAnimator : BirdAnimatorCore {
    public Sprite birdDashSprite;

    void Update() {
        UpdateDashSprite();
    }

    private void UpdateDashSprite() {
        if (birdDash.isDashing) {
            animator.enabled = false;   
            spriteRenderer.sprite = birdDashSprite;
            Debug.Log("biags");
        } else {
            animator.enabled = true;   
        }
    }
}