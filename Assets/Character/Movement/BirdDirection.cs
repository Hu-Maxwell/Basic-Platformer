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
        if (Input.GetKeyDown(KeyCode.A))
        {
            lookingDirectionX = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            lookingDirectionX = 1; 
        }
        
        // not using this because it has smoothing - also, it gets set to 0.
        // lookingDirectionX = Input.GetAxisRaw("Horizontal");
    }
}
