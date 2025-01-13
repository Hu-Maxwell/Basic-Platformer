using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    public Image container;
    public Sprite fill;
    public Sprite empty;
    
    public bool tick; 

    void Update() {
        if (tick) {
            container.sprite = empty;
        } else {
            container.sprite = fill;
        }
    }
}