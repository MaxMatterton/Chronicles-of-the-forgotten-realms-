using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Traps : MonoBehaviour
{
    private Rigidbody2D MyBody;

    public PlayerHealth playerHealth;

    float Timer = Mathf.Infinity;

    [Header("Spear")]
    public bool Spear; // Enable this to activate movement
    private Vector3 InitialPosition;
    public float SpearWaitTime;
    private bool isMoving = false;
    public float distance;
    public float Speed;

    [Header("Arrow Properties")]
    public bool Arrow;
    public GameObject projectilePrefab; // Assign in the Inspector
    public float launchSpeed; // Speed of the launched arrow
    public float maxDistance; // Distance before destroying arrow
    public float ArrowWaitTime;

    [Header("Spike Ball Properties")]// Spike Ball Properties
    public bool SpikeBall;
    public Transform pointA;  // First point
    public Transform pointB;  // Second point
    public float speed = 1.5f;
    public bool reverse;
    


    void Awake()
    {
        MyBody = GetComponent<Rigidbody2D>();
        InitialPosition = transform.position;
        
    }

    void Update()
    {
        Timer +=Time.deltaTime;

        if (Spear && !isMoving)
        {
            StartCoroutine(SpearTrap());
        }
        else if (Arrow)
        {
            if (Timer >= ArrowWaitTime)
            {
                Timer = 0;
                LaunchProjectile(Vector2.down);
            }
        }
        else if (SpikeBall)
        {
            SpikeBallMovement();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }

    public void SpikeBallMovement()
    {
        if (reverse)
        {
            // Spike Ball trap
            float t = Mathf.PingPong(Time.time * speed, 1);
            transform.position = Vector2.Lerp(pointB.position,pointA.position, t); // Moves smoothly between points
        }
        else if (!reverse)
        {
            // Spike Ball trap
            float t = Mathf.PingPong(Time.time * speed, 1);
            transform.position = Vector2.Lerp(pointA.position, pointB.position, t); // Moves smoothly between points
        }
        
    }

    private IEnumerator SpearTrap()
    {
        isMoving = true;

        // Move Up
        Vector3 targetUp = InitialPosition + Vector3.up * distance;
        yield return StartCoroutine(MoveToPosition(targetUp));

        // Wait
        yield return new WaitForSeconds(SpearWaitTime);

        // Move Down
        yield return StartCoroutine(MoveToPosition(InitialPosition));

        // Wait again before repeating
        yield return new WaitForSeconds(SpearWaitTime);

        isMoving = false;
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }

    public void LaunchProjectile(Vector2 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned!");
            return;
        }
        // Instantiate the arrow
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        // Add movement to the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * launchSpeed; // Moves arrow in the given direction
        }

        // Destroy the arrow after traveling maxDistance
        StartCoroutine(DestroyAfterDistance(projectile));
    }

    private IEnumerator DestroyAfterDistance(GameObject obj)
    {
        Vector3 startPos = obj.transform.position;
        while (Vector3.Distance(startPos, obj.transform.position) < maxDistance)
        {
            yield return null;
        }
        Destroy(obj);
    }
    
}
