using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneric : MonoBehaviour, IUpdatable, IInterctable, IDamagable
{
    public float Speed;
    public float TimeToStopAfterHitPlayer = 3f;

    public int Life;

    private int currentLife;
    private float timerToStop;

    private float stunCurrentTime;
    private bool foundPlayer;
    private Vector2 startPosition;
    private Transform playerTf;

    public void FoundPlayer()
    {
        foundPlayer = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stunCurrentTime > 0)
            return;

        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            timerToStop = 3f;
            character.TakeDamage(transform.position, DamageSpecialEffect.None, 1);
        }
    }

    public void UpdateObj()
    {
        if (stunCurrentTime > 0)
        {
            stunCurrentTime -= Time.deltaTime;
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

    public void SetupOnStartLevel()
    {
        startPosition = transform.position;
        playerTf = GameplayController.instance.GetPlayer().transform;
    }

    public void ResetObj()
    {
        transform.position = startPosition;
        foundPlayer = false;
        currentLife = Life;
    }

    public void TakeDamage(Vector2 damageOrigin, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None, int ammount = 1)
    {
        if(damageSpecialEffect == DamageSpecialEffect.Stun)
        {
            stunCurrentTime = 3;
        }
    }
}


public enum EnemyState
{
    Patrol,
    Chase
}
