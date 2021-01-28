using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour
{
    public float Speed;

    public Transform[] Patrol;


    private int patrolPoint;
    private EnemyState state;
    private Vector2 lastFacingDirection;
    private void Awake()
    {

    }

    public void FoundPlayer()
    {
        Debug.Log("PlayerFound");
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                {

                    break;
                }
        }
    }

}


public enum EnemyState
{
    Patrol,
    Chase
}
