using UnityEngine;
using Game.Interfaces;

// TODO: reverse direction when wall is touched
// TODO: if 0 param, set a default move speed and infinite max distance
// TODO: make enemy movement not awkawrd and robotic

// to use: 
// PatrolMovement(float speed, float maxDistance) 
// where maxDistanace is the furthest the enemy will travel before switching direction 

public class PatrolMovement : IMovementBehavior {
    private float _speed;
    private float _maxDistance;
    private float _currentDistance;

    public PatrolMovement(float speed, float maxDistance) {
        _speed = speed;
        _maxDistance = maxDistance;
        _currentDistance = 0f;
    }

    // TODO: also take a param for how much the enemy should move
    public void Move(Enemy enemy) {
        Vector3 newPosition = enemy.transform.position;
        newPosition.x += _speed;
        _currentDistance += _speed;

        if (Mathf.Abs(_currentDistance) >= _maxDistance) {
            _speed *= -1; 
        }

        enemy.transform.position = newPosition;
    }
} 