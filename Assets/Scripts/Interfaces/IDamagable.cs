using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    void TakeDamage(Vector2 damageOrigin, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None, int ammount = 1);   
}

public enum DamageSpecialEffect
{
    None,
    Stun,
}