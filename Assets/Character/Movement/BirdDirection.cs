using UnityEngine;

public class BirdDirection : BirdCore
{
    #region vars
    public float lookingDirectionX = 1;
    #endregion

    void Update()
    {
        if (!birdDash.isDashing)
        {
            UpdateDirection();
        }
    }

    // not really worth to separate the input and the actual movement function because it's just one line
    public void UpdateDirection()
    {
        if (Input.GetKey(KeyCode.A))
        {
            lookingDirectionX = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            lookingDirectionX = 1; 
        }
        
        // not using this because it has smoothing - also, it gets set to 0.
        // lookingDirectionX = Input.GetAxisRaw("Horizontal");
    }
}
