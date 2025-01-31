using UnityEngine;
using Game.Interfaces;

// this is a script that is attached to the skeleton prefab 

public class SkeletonBase : Enemy {
    public bool DebugTickAttack;
    public bool DebugTickMove;  

    private void Start() {
        // inject default behaviors
        SetAttackBehavior(new MeleeAttack());
        SetMovementBehavior(new PatrolMovement());
    }

    void Update() { // does having Update in this class adhere to SOLID principles? 
        CheckMovementSwitch(); 
    }

    void FixedUpdate() {
        Move(this); 
    }


    private void CheckMovementSwitch() {
        // if player is close enough, switch to chase
        // if not close enough, switch to patrol
    }
}
