using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float range2;
    [Space] 
    [SerializeField] private int damage;
    [SerializeField] private float damage2;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private float colliderDistance2;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Back Collider Parameters")]
    [SerializeField] private float BackcolliderDistance;
    [SerializeField] private float Backrange;

    [Header("Player Layer")]
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private LayerMask playerLayer;

    //References
    private Animator anim;
    private PlayerHealth Health;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSightAttack1())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        else if (PlayerInSightAttack2())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack2");
            }
        }
        else if (BehindEnemy())
        {
            enemyPatrol.lookatplayer();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSightAttack1();
            enemyPatrol.enabled = !PlayerInSightAttack2();

    }

    private bool PlayerInSightAttack1()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            Health = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }
    private bool PlayerInSightAttack2()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range2 * transform.localScale.x * colliderDistance2,
            new Vector3(boxCollider.bounds.size.x * range2, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            Health = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }
    private bool BehindEnemy()
    {
        RaycastHit2D Behindhit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * Backrange * transform.localScale.x * BackcolliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (Behindhit.collider != null)
            Health = Behindhit.transform.GetComponent<PlayerHealth>();

        return Behindhit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * Backrange * transform.localScale.x * BackcolliderDistance,
            new Vector3(boxCollider.bounds.size.x * Backrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range2 * transform.localScale.x * colliderDistance2,
            new Vector3(boxCollider.bounds.size.x * range2, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSightAttack1())
            Health.TakeDamage(damage,true);
        else if (PlayerInSightAttack2())
            Health.TakeDamage(damage2,true);
    }
}
