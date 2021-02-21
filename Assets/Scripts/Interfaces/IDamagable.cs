using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    void TakeDamage(Vector2 damagerPosition, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None);   
}

public enum DamageSpecialEffect
{
    None,
    Stun,
    Knockback,
    BreakArmor,
}