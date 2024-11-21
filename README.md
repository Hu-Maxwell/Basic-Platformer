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

**THIS PART IS A DRAFT! It is not written well or organized. **

There are three kinds of jumps: 
- Normal jump
- Double jump
- Wall jump

The functions for each of these features are relatively simple. For normal and double jump, a force is applied to the rigidbody upwards with a magnitude of `jumpForce`. For wall jump, two forces are applied: an upward force, and a horizontal force in the opposite direction the character is facing. 

However, the difficulty comes in tweaking these jumps to make them feel 1. responsive, 2. bouncy, and 3. smooth. To achieve this, we will implement several features. 

- down force when space is released
- buffer
- coyote time
- velocity slowdown at apex of jump

Down force: 

- exerts a downward force when the character releases space
- this helps the player control the height of the jump, making it feel responsive
- the down force is not applied if the user is already moving downawrds, or if the user is already 3/4s of the height.
- the 3/4ths of the height is just a timer instead, calculated using the equation (insert equation) 

Buffer: 

- if the user presses space prematurely (before landing), the user may feel like the game didn't register their input.
- to fix this, we will implement a buffer. there are two ways to do this:
    - timer
    - distance

- the timer begins if the the user has used both their jumps and presses space. this timer is set to .2 seconds (you can modify if needed). if, in any frame during the timer, the user touches the ground, the user will immediately jump again.

- the next method casts a ray downwards that's a bit longer than the character.
    - if the user has both jumped and doublejumped, and if jump is pressed when the character is ray dist from floor, it will jump once isGrounded is true
	- if the user has jumped and is too close to the floor, it will skip a double jump and wait until the floor is touched.

    - this was tested; there are many flaws with this method. 
		- firstly, it doesn't take into account a dash - if the user presses dash then jump very early on in the dash, and is close to the floor, then the user might get a very very delayed jump 
		- it isn't really scalable with slower fall speeds. if the user is falling really slow, the jump buffer might feel really delayed
		- and lastly, the functionality of switching a double jump to a normal jump isn't really too useful. not many players will try to double jump last second

Coyote time: 

- if the user wants to make a jump that is a very long distance, and presses jump at the very last second, they might walk off the ledge instead of jumping. to make fairer times where inputs need to be tight, we will implement coyote time.
- coyote time is a short duration after the character walks off the ground. for that duration, the user can still execute their first jump.
- this is simple to implement, and only takes one boolean statement.

```
if (timeSinceOffGround < coyoteTime) {
    NormalJump(); 
}
```

### Dash: 

TODO


### Additonal Notes:

Remember to: 
- add a physics material to either the character or level that sets friction to 0
- set the character's rigidbody to interpolate (extrapolate works too, but interpolate is safer. extrapolate predicts future values, while interpolate predicts values within a range.)
- lock the character's z axis
