using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsCrab : MonoBehaviour
{
    public Vector2 KnockbackForce;
    private PlataformMove moveScript;

    private Vector2 startPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            character.TakeDamage(transform.position, KnockbackForce);

            if (moveScript == null)
                moveScript = GetComponentInParent<PlataformMove>();

            moveScript.SetNextDestination();
        }
    }

}
