using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar Hb;
    public GameObject Ds;
    Animator anim;

    bool Dead;

    void Start()
    {
        anim = GetComponent<Animator>();

        Hb.setmaxHealth(playermovement.playerstats.MaxHealth);
        playermovement.playerstats.Health = playermovement.playerstats.MaxHealth;

    }
    void Update()
    {
        Hb.sethealth(playermovement.playerstats.Health);
    }

    public void TakeDamage(float damage)
    {
        playermovement.playerstats.GetHurt(damage);
        Hb.sethealth(playermovement.playerstats.Health);

        if (playermovement.playerstats.Health > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            if (!Dead)
            {

                if (GetComponent<playermovement>() != null)

                {
                    anim.SetTrigger("dead");
                    Ds.SetActive(true);
                    GetComponent<playermovement>().enabled = false;
                    Dead = true;
                }
            }
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            TakeDamage(playermovement.playerstats.MaxHealth);
        }
    }
        
}
