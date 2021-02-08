using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public int damageAmmount = 1;
    public Vector2 KnockbackForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            character.TakeDamage(transform.position, KnockbackForce, damageAmmount);
        }
    }
}
