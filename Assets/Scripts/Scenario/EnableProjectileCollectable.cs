using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableProjectileCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponentInParent<CharacterController>();

        if (character != null)
        {
            character.EnableProjectileToBeUsed();
            gameObject.SetActive(false);
        }
    }
}
