using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : EnemyGeneric
{
 
    public float Speed = 5f;

    private DrawPoint[] points;
    private int moveIndex;
    private Vector2 nextLocation;
    private Transform cachedTf;

    private Vector2 startPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if(DealDamage(character))
               SetNextDestination();
        }
    }



   
    private void Awake()
    {
        cachedTf = transform;
        points = cachedTf.parent.GetComponentsInChildren<DrawPoint>(true);
        nextLocation = points[0].transform.position;
    }

    private void SetNextDestination()
    {
        moveIndex++;

        if (points == null)
            points = GetComponentsInChildren<DrawPoint>(true);

        if (moveIndex >= points.Length)
            moveIndex = 0;

        nextLocation = points[moveIndex].transform.position;
    }

    public override void SetupOnStartLevel()
    {
     
    }

    public override void ResetObj()
    {
        moveIndex = 0;
        gameObject.SetActive(true);
    }

    public override void UpdateObj()
    {
        if (disableTime > 0)
        {
            disableTime -= Time.deltaTime;
            return;
        }

        if (Vector2.Distance(cachedTf.position, nextLocation) < 0.05f)
        {
            SetNextDestination();
        }
        else
        {
            cachedTf.position = Vector2.MoveTowards(cachedTf.position, nextLocation, Speed * Time.deltaTime);
        }
    }

    public override void TakeDamage(Vector2 damageOrigin, DamagerType damagerType,  int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        base.TakeDamage(damageOrigin, damagerType,  ammount, damageSpecialEffect);
        SoundController.instance.PlayAudioEffect("CrabDie");
        gameObject.SetActive(false);
    }

    public override bool DealDamage(CharacterInstance character)
    {
        SoundController.instance.PlayAudioEffect("CrabAttack");
        return base.DealDamage(character);
    }

}
