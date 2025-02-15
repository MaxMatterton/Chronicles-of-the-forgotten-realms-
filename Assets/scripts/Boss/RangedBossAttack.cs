using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    public int damage;

    [Header("Behind Enemy")]
    [SerializeField] private float BackcolliderDistance;
    [SerializeField] private float Backrange;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject FireballPreFab;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() == true && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("rangedAttack");
        }
        else if (BehindEnemy())
        {
            enemyPatrol.lookatplayer();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
        
        
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;

        // 🔹 Get boss's facing direction
        float bossDirection = Mathf.Sign(transform.localScale.x);
    
        // 🔹 Instantiate fireball and set its direction
        GameObject fireball = Instantiate(FireballPreFab, firepoint.position, Quaternion.identity);
        fireball.GetComponent<Projectile>().ActivateProjectile(bossDirection);
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private bool BehindEnemy()
    {
        float direction = transform.localScale.x > 0 ? 1 : -1;
        RaycastHit2D Behindhit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * Backrange * direction * BackcolliderDistance,
            new Vector3(boxCollider.bounds.size.x * Backrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return Behindhit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * Backrange * transform.localScale.x * BackcolliderDistance,
            new Vector3(boxCollider.bounds.size.x * Backrange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
