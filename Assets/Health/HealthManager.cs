using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;

public class HealthManager : MonoBehaviour {
    
    public HealthDisplay healthDisplay;

    private int maxHealth;
    private int curHealth; 
    private float timeSinceLastDamage; 

    public bool tick = false;

    void Update() {
        if (tick) {
            ModifyMaxHealth(1);
            tick = false; 
        }
    }

    // checks if player is dead (zero hearts)
    private void CheckDeath() {
        if (curHealth == 0) {
            Debug.Log("dead");
        }
    }

    // modifies total health
    public void ModifyMaxHealth(int amount) {
        maxHealth+= amount; 
        healthDisplay.UpdateHealthDisplay(curHealth, maxHealth); 
    }

    // modifies current health - by default removes one
    public void ModifyCurHealth(int amount) {
        curHealth += amount; 
        healthDisplay.UpdateHealthDisplay(curHealth, maxHealth); 
    }
}