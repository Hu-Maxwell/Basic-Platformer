using UnityEngine;

public class BirdDirection : BirdCore
{
    // vars
    public float lookingDirectionX = 1;

    void Update()
    {
        UpdateDirection();
    }

    public void UpdateDirection()
    {
        lookingDirectionX = Input.GetAxisRaw("Horizontal");
    }
}
