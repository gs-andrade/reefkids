using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<IDamagable>();

        if (character != null)
        {
            character.TakeDamage(transform.position, DamagerType.Enemy, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<IDamagable>();

        if (character != null)
        {
            character.TakeDamage(transform.position, DamagerType.Enemy, 1);
        }
    }
}
