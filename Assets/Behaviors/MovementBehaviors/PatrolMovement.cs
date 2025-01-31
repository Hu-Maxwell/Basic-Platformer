using UnityEngine;
using Game.Interfaces;

public class PatrolMovement : IMovementBehavior {
    // temp vars
    private float speed = .1f;
    private float dist; 

    // TODO: also take a param for how much the enemy should move
    public void Move(Enemy enemy) {
        Vector3 newPosition = enemy.transform.position;

        newPosition.x += speed;
        dist += speed; 

        if (dist > 5.5f || dist < -5.5f) {
            speed *= -1;
        }

        enemy.transform.position = newPosition;
    }
} 