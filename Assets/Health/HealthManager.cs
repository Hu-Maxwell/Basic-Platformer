using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;

public class HealthManager : MonoBehaviour {
    
    public HealthDisplay healthDisplay;

    #region cur / max health vars
    private int maxHealth;
    private int curHealth; 

    [SerializeField] private int startingCurHealth;
    [SerializeField] private int startingMaxHealth;
    
    public int MaxHealth => maxHealth; 
    public int CurHealth => curHealth; 
    #endregion

    private float timeSinceLastDamage; 

    public bool maxTick_ = false;
    public bool curTick_ = false; 

    void Awake() {
        // Awake, because it's before Start. if Start is used, healthDisplay will think maxHealth and curHealth is 0.  
        curHealth = startingCurHealth;
        maxHealth = startingMaxHealth; 
    }

    void Update() {
        DebugHealth(); 
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

    private void DebugHealth() {
        if (maxTick_) {
            ModifyMaxHealth(1);
            maxTick_ = false; 
        }

        if (curTick_) {
            ModifyCurHealth(1);
            curTick_ = false; 
        }
    }
}