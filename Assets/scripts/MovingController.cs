using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    public float moveDistance = 2f; // Distance to move up and down
    public float moveSpeed = 2f; // Speed of the movement
    private Vector3 startPosition;
    private bool movingUp = true;
    private bool Vertical;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vertical)
        {
            if (movingUp)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                if (transform.position.y >= startPosition.y + moveDistance)
                {
                    movingUp = false; // Change direction
                }
            }
            else
            {
                transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
                if (transform.position.y <= startPosition.y)
                {
                    movingUp = true; // Change direction
                }
            }
        }
        else
        {
            if (movingUp)
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                if (transform.position.x >= startPosition.x + moveDistance)
                {
                    movingUp = false; // Change direction
                }
            }
            else
            {
                transform.position -= Vector3.right * moveSpeed * Time.deltaTime;
                if (transform.position.x <= startPosition.x)
                {
                    movingUp = true; // Change direction
                }
            }
        }
        
    }
}
