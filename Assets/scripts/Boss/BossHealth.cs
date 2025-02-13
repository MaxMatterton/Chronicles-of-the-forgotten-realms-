using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
     public HealthBar Hb;
    Animator anim;
    public playermovement pm;

    public int MaxHealth = 100;
    public float CurrentHealth;
    public float DeathDelay;
    bool Dead;

    //particle system
    [SerializeField] private ParticleSystem damageParticles;
    private ParticleSystem damageParticlesInstance;

    void Start()
    {
        anim = GetComponent<Animator>();

        Hb.setmaxHealth(MaxHealth);
        CurrentHealth = MaxHealth;

    }

    public void TakeDamage(float damage,Vector2 AttackDirection)
    {
        CurrentHealth -= damage;
        Hb.sethealth(CurrentHealth);

        if (CurrentHealth > 0)
        {
            DamageParticles(AttackDirection);
        }
        else
        {
            if (!Dead)
            {
                anim.SetTrigger("dead");
                Invoke("DeadEnemy",DeathDelay);
                Dead = true;
                GetComponent<EnemyPatrol>().enabled = false;
            }
        }

    }
    public void DeadEnemy()
    {
        if (this.gameObject.CompareTag("Boss1"))
        {
            pm.KeyCollected = true;
            Destroy(this.gameObject);
        }
        else{
            Destroy(this.gameObject);
        }
    }

    public void DamageParticles(Vector2 AttackDirection)
    {
        Quaternion rot = Quaternion.FromToRotation(Vector2.right,AttackDirection);
        damageParticlesInstance = Instantiate(damageParticles, transform.position, Quaternion.identity);
    }
}
