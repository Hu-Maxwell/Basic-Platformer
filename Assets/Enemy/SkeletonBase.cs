using UnityEngine;
using Game.Interfaces;
using System;
using Unity.VisualScripting;

// this is a script that is attached to the skeleton prefab 

public class SkeletonBase : Enemy {
    [SerializeField] private Rigidbody2D rb; // this this a clean way to get rb in here? 
    [SerializeField] private Rigidbody2D enemyRb; // TEMPORARY  

    private void Start() {
        // inject default behaviors
        SetAttackBehavior(new MeleeAttack());
        SetMovementBehavior(new PatrolMovement(.1f, 3f)); // TODO: patrol movement should also be able to take nothing as a param in future, so change later
    }

    void Update() { // does having Update in this class adhere to SOLID principles? 
        CheckMovementSwitch(); 
    }

    void FixedUpdate() {
        Move(this); 
    }

    private void CheckMovementSwitch() {
        if (Math.Abs(rb.position.x - transform.position.x) < 2.0f) {
            SetMovementBehavior(new ChaseMovement(rb, enemyRb)); 
            Debug.Log("switched");
        }
    }
}
