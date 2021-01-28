using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private EnemyGeneric genericMonster;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (genericMonster == null)
            genericMonster = GetComponentInParent<EnemyGeneric>();

        genericMonster.FoundPlayer();
    }
}
