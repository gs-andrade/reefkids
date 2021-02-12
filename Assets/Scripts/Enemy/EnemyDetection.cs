using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyChase genericMonster;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<CharacterInstance>();

        if (player != null)
        {
            if (genericMonster == null)
                genericMonster = GetComponentInParent<EnemyChase>();

            genericMonster.FoundPlayer();
        }
    }
}
