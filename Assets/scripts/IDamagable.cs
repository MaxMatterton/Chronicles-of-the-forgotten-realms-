using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void Damage(float damageamount);

    public bool HastakenDamage { get; set; }
}
