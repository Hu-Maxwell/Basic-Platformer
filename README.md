# Basic Platformer

This is a simple platformer with well organized, scalable, and modular code. Made for both my own future reference and to teach to friends. 

Features: 
- orderly file organization 
- complex and well tuned movement
- TODO: animations
- TODO: different levels
- TODO: level obstacle design
- TODO: enemies

--- 

## Movement:

The character has three main movement capabilities: walk, jump, and dash. 

### Walk:  

```
public void Walk(float direction)
{
    float targetSpeed = direction * moveSpeed;
    float speedDiff = targetSpeed - rb.linearVelocity.x;
    float accelRate = (Mathf.Abs(targetSpeed) > .01) ? accelAmount : decelAmount;

    float forceApplied = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

    rb.AddForce(forceApplied * Vector2.right);
}
```

This walking system uses forces to accelerate, decelerate, and maintain speed.  

1. `targetSpeed`

    - The `targetSpeed` is based off of the current input: moving left, right, or standing still, and the movespeed variable. 

2. `speedDiff`
    
    - The `speedDiff` is based off the target speed subtracted by the current speed. 

3. `accelRate`

    - The `accelRate` determines whether to use the acceleration or deceleration rate based off of whether or not the targetSpeed is 0.
        - If it is 0, it decelerates.
        - Otherwise, it accelerates.
    - Note: .01 is used instead of 0 to account for floating-point errors. 

### Force calculation: 

Finally, `forceApplied` follows the equation:

$$
|F| = (|Δv| \cdot \text{accelRate})^{\text{velPower}}
$$

$$
F = |F| \cdot \text{direction}
$$

- Since we want the character to accelerate smoothly, we square the value by `velPower`. This should scale between 0-1.
 
- The formula for amount of time required to accelerate to full speed is: 

$$
|F| = (|Δv| \cdot \text{accelRate})^{\text{velPower}}
$$



### Jump:  

To be written. 


### Dash: 

TODO
