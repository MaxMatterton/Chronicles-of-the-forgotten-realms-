using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public HealthBar Hb;
    private Animator anim;
    public playermovement pm;

    public int MaxHealth = 100;
    public float CurrentHealth;
    public float DeathDelay;
    private bool Dead;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    private RangedBossAttack bossAttack;
    private EnemyPatrol enemyPatrol;
    private Collider2D bossCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        bossAttack = GetComponent<RangedBossAttack>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        bossCollider = GetComponent<Collider2D>();

        CurrentHealth = MaxHealth;
        Hb.setmaxHealth(MaxHealth);
    }

    public void TakeDamage(float damage, Vector2 AttackDirection)
    {
        CurrentHealth -= damage;
        Hb.sethealth(CurrentHealth); // Update health bar after setting health

        if (CurrentHealth > 0)
        {
            DamageParticles(AttackDirection);
            Hb.sethealth(CurrentHealth);
        }
        else
        {
            if (!Dead)
            {
                Dead = true;
                anim.SetTrigger("dead");

                // Disable attack & movement
                if (bossAttack != null) bossAttack.enabled = false;
                if (enemyPatrol != null) enemyPatrol.enabled = false;

                Invoke("DeadEnemy", DeathDelay);
            }
        }
    }

    private void DeadEnemy()
    {
        if (gameObject.CompareTag("Boss1"))
        {
            pm.KeyCollected = true;
        }

        Destroy(gameObject);
    }

    private void DamageParticles(Vector2 AttackDirection)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, AttackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}
