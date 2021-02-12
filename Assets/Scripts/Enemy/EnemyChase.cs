using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : EnemyGeneric
{
    public float Speed;
    public float TimeToStopAfterHitPlayer = 3f;

    public int Life;

    private float timerToStop;

    private bool foundPlayer;
    private Vector2 startPosition;
    private Transform playerTf;

    public void FoundPlayer()
    {
        foundPlayer = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (StunDisableTime > 0)
            return;

        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            timerToStop = 3f;
            DealDamage(character);
        }
    }

    public override void UpdateObj()
    {
        if (disableTime > 0)
        {
            disableTime -= Time.deltaTime;
            return;
        }


        if (foundPlayer && playerTf != null)
        {
            if (timerToStop <= 0)
                transform.position = Vector2.MoveTowards(transform.position, playerTf.position, Speed * Time.deltaTime);
            else
            {
                timerToStop -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, startPosition, Speed/3 * Time.deltaTime);
            }

        }
    }

    public override void SetupOnStartLevel()
    {
        startPosition = transform.position;
        playerTf = GameplayController.instance.GetPlayer().transform;
    }

    public override void ResetObj()
    {
        transform.position = startPosition;
        foundPlayer = false;
    }
}


public enum EnemyState
{
    Patrol,
    Chase
}
