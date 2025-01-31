using UnityEngine;
using Game.Interfaces;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour {
    protected IMovementBehavior movementBehavior; 
    protected IAttackBehavior attackBehavior;

    [SerializeField] private float health = 100f; 
    [SerializeField] private float speed = 1f; 
    [SerializeField] private int damage = 1; 

    public void SetMovementBehavior(IMovementBehavior newMovement) {
        movementBehavior = newMovement;
    }

    public void SetAttackBehavior(IAttackBehavior newAttack) {
        attackBehavior = newAttack; 
    }

    public void Move(Enemy enemy) {
        movementBehavior?.Move(enemy);
    }

    public void Attack() {
        attackBehavior?.Attack();
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    private void Die() {
        Destroy(gameObject);
    }
}