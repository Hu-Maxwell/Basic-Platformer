using UnityEngine;
using Game.Interfaces; 

public class Enemy : MonoBehaviour {
    protected IMovementBehavior movementBehavior; 
    protected IAttackBehavior attackBehavior;

    [SerializeField] private float health = 100f; 

    public void SetMovementBehavior(IMovementBehavior newMovement) {
        movementBehavior = newMovement;
    }

    public void SetAttackBehavior(IAttackBehavior newAttack) {
        attackBehavior = newAttack; 
    }

    public void Move() {
        movementBehavior?.Move();
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