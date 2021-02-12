using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyGeneric : MonoBehaviour, IUpdatable, IInterctable, IDamagable
{
    [Header("Generic Enemy Attributes")]
    public bool StunOnDamageTake;
    public float StunDisableTime = 3f;

    protected float disableTime;
    public abstract void ResetObj();

    public abstract void SetupOnStartLevel();

    public virtual void TakeDamage(Vector2 damageOrigin, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None, int ammount = 1)
    {
        if (StunOnDamageTake)
            disableTime = StunDisableTime;
    }

    public abstract void UpdateObj();

    public virtual void DealDamage(CharacterInstance character)
    {
        if(disableTime <= 0)
        {
            character.TakeDamage(transform.position);
        }
    }

}
