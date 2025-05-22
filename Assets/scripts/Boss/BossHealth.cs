using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public HealthBar Hb;
    private Animator anim;

    public int MaxHealth = 100;
    public float CurrentHealth;
    public float DeathDelay;
    private bool Dead;

    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    private Collider2D bossCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        bossCollider = GetComponent<Collider2D>();

        CurrentHealth = MaxHealth;
        Hb.setmaxHealth(MaxHealth);
    }

    private void FixedUpdate()
    {
        Hb.sethealth(CurrentHealth);    
    }

    public void TakeDamage(float damage, Vector2 AttackDirection)
    {
        CurrentHealth -= damage;
        
        if (CurrentHealth > 0)
        {
            DamageParticles(AttackDirection);
        }
        else if (!Dead)
        {
            if (GetComponent<BossAttack>() != null)
            {
                anim.SetTrigger("dead");
                Dead = true;
                Invoke("DeadEnemy",DeathDelay);
                GetComponent<RangedBossAttack>().enabled = false;
                GetComponent<EnemyPatrol>().enabled = false;
            }
            else if(GetComponent<RangedBossAttack>() != null)
            {
                anim.SetTrigger("dead");
                Dead = true;
                Invoke("DeadEnemy",DeathDelay);
                GetComponent<RangedBossAttack>().enabled = false;
                GetComponent<EnemyPatrol>().enabled = false;
            }
        }
    }

    private void DeadEnemy()
    {
        Destroy(gameObject);
    }

    private void DamageParticles(Vector2 AttackDirection)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, AttackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}
