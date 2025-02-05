using UnityEngine;
using Game.Interfaces;
using UnityEditor.Callbacks;

public class ChaseMovement : IMovementBehavior {
    [SerializeField] private Rigidbody2D rb;  // the players rb
    [SerializeField] private Rigidbody2D enemyRb; 

    private float speed = .1f; 

    public ChaseMovement(Rigidbody2D _rb, Rigidbody2D _enemyRb) {
        rb = _rb; // this sucks
        enemyRb = _rb; 
        // _speed = speed;
    }


    public void Move(Enemy enemy) {
        Vector2 direction = (enemy.transform.position - (Vector3)rb.position).normalized;
        enemyRb.linearVelocity = new Vector2(direction.x * speed, enemyRb.linearVelocityY); // what why does this make the player move towards the enemy???
    }
}