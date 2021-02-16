using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeStunnedProjectile : MonoBehaviour, IDamagable
{
    public void TakeDamage(Vector2 damagerPosition, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        Destroy(gameObject);
    }

}
