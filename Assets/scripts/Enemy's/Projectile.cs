using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 3f;
    private float lifetime;
    private Animator anim;
    private PolygonCollider2D coll;
    public float damage;

    private bool hit;
    private float direction; // 🔹 Stores projectile direction

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<PolygonCollider2D>();
    }

    public void ActivateProjectile(float bossDirection)
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;

        // 🔹 Set projectile direction based on boss facing
        direction = bossDirection;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, 
                                           transform.localScale.y, 
                                           transform.localScale.z);
    }

    private void Update()
    {
        if (hit) return;

        // 🔹 Move projectile in the correct direction
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        // Destroy after a certain time
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return; // Prevent multiple triggers
        hit = true;

        coll.enabled = false;

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage,true);
            

        }

        if (anim != null)
        {
            anim.SetTrigger("explode"); // Play explosion animation
            Destroy(gameObject, 0.5f);  // Destroy after animation finishes
        }
        else
        {
            Destroy(gameObject); // Destroy instantly if no animation
        }
    }
}
