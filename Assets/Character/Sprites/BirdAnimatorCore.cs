using System;
using UnityEngine;

public class BirdAnimatorCore : MonoBehaviour {
    public Animator animator; 
    public Rigidbody2D rb; 
    public SpriteRenderer spriteRenderer;

    void Update() {
        UpdateSpeed();
    }

    private void UpdateSpeed() {
        animator.SetFloat("speed", Math.Abs(rb.linearVelocityX)); 
    }
}