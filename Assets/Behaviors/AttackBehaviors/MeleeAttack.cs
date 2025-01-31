using UnityEngine;
using Game.Interfaces;

public class MeleeAttack : IAttackBehavior {
    private Transform target;

    public void Attack() {
        Debug.Log("melee attack");
    }
} 