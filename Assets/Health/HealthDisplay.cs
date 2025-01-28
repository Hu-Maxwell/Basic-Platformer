using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HealthDisplay : MonoBehaviour {
    public HealthManager healthManager;

    public GameObject heartPrefab; 
    public Transform heartContainer;
    public Sprite fill; 
    public Sprite empty;

    void Start() {
        UpdateHealthDisplay(healthManager.CurHealth, healthManager.MaxHealth); // todo: should ASK health manager first for maxHealth and curHealth 
    }

    // updates health display 
    public void UpdateHealthDisplay(int currentHealth, int maxHealth) {
        foreach (Transform child in heartContainer) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxHealth; i++) {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            Image heartImage = heart.GetComponent<Image>();
            heartImage.sprite = i < currentHealth ? fill : empty; 
        }
    }
}