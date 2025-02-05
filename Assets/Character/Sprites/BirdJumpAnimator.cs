using System;
using UnityEngine;

public class BirdJumpAnimator : BirdAnimatorCore {
    public Sprite upwardsJumpSprite;
    public Sprite downwardsJumpSprite;

    void Update() {
        UpdateJumpSprite();
    }

    private void UpdateJumpSprite() {
        if (birdDash.isDashing) {
            return; 
        }
        
        if (Math.Abs(rb.linearVelocityY) > 1.0f) {
            animator.enabled = false;
        } else {
            animator.enabled = true;
        }

        if (rb.linearVelocityY >= 0) {
            spriteRenderer.sprite = upwardsJumpSprite;
        } else {
            spriteRenderer.sprite = downwardsJumpSprite;
        }
    }
}