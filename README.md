# Basic Platformer

This is a simple platformer with heavily organized code. Made for both my own future reference and to teach to friends. 

Features: 
- orderly file organization 
- complex and well tuned movement
- TODO: animations
- TODO: different levels
- TODO: level obstacle design
- TODO: enemies

--- 

### Movement:

The character has three main movement capabilities: walk, jump, and dash. 

**Walking**: 

```
    public void Walk(float direction)
    {
        float targetSpeed = direction * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > .01) ? accelAmount : decelAmount;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
    }
```

This walking system uses forces to accelerate, decelerate, and maintain speed. To calculate the force, it first needs to calculate these variables: 
1. target speed
2. speed difference
3. acceleration rate

The target speed is based off of the current input: moving left, right, or standing still, and the movespeed variable. 
Then, it calculates the difference in speed is based off the target speed subtracted by the current speed. 
