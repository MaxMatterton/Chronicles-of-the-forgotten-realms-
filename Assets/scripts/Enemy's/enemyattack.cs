using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

public class enemyattack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] float attackCooldown;
    [SerializeField] float range;

    [Header("Collider Parameters")]
    [SerializeField] float colliderDistance;
    [SerializeField] BoxCollider2D boxCollider;

    [Header("Back ollider Parameters")]
    [SerializeField] float BackcolliderDistance;
    [SerializeField] float Backrange;

    [Header("Player Layer")]
    [SerializeField] LayerMask playerLayer;
    float cooldownTimer = Mathf.Infinity;

    [Header("Enemy Type")]
    [SerializeField] int enemyType;
    float EnemyDamage;

    //References
    Animator anim;
    PlayerHealth Playerhealth;
    EnemyPatrol enemyPatrol;

    public float CalculateEnemyDamage(float damage)
    {
        return Mathf.Max(playermovement.playerstats.MaxHealth/10 - damage , 1);
    }


    public void SetType()
    {
        if (enemyType == 0)
        {
            EnemyDamage = 10;
        }
        else if (enemyType == 1)
        {
            EnemyDamage = 5;
        }
        else if (enemyType == 2)
        {
            EnemyDamage = 0;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        SetType();
        
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("attack");
            }
        }
        else if (BehindEnemy())
        {
            enemyPatrol.lookatplayer();
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            Playerhealth = hit.transform.GetComponent<PlayerHealth>();

        return hit.collider != null;
    }
    private bool BehindEnemy()
    {
        RaycastHit2D Behindhit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * Backrange * transform.localScale.x * BackcolliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (Behindhit.collider != null)
            Playerhealth = Behindhit.transform.GetComponent<PlayerHealth>();

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

    private void DamagePlayer()
    {
        if (PlayerInSight())
            Playerhealth.TakeDamage(CalculateEnemyDamage(EnemyDamage),true);
    }
    
    
}
