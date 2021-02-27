using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour, IUpdatable, IInterctable, IDamagable
{
    public float Speed = 5f;
    public int LifeMax = 3;
    public float BossStunDuration = 5f;
    public float RecoveryTime = 3f;

    public BoxCollider2D[] Plataforms;
    public GameObject AreaDamageGO;

    private float currentSpeed;

    private float bossStunTimer;
    private int lifeCurrent;

    private BoxCollider2D[] areaDamageColliders;

    private DrawPoint[] points;
    private int moveIndex;
    private Vector2 nextLocation;
    private Transform cachedTf;
    private BossState state;
    private SpriteRenderer renderer;

    private void SetNextDestination()
    {
        moveIndex++;

        if (points == null)
            points = transform.parent.GetComponentsInChildren<DrawPoint>(true);

        if (moveIndex >= points.Length)
            moveIndex = 0;

        nextLocation = points[moveIndex].transform.position;
    }

    public void SetupOnStartLevel()
    {
        renderer = GetComponent<SpriteRenderer>();
        cachedTf = transform;
        points = cachedTf.parent.GetComponentsInChildren<DrawPoint>(true);
        nextLocation = points[0].transform.position;
        lifeCurrent = LifeMax;
        state = BossState.Normal;

        if(areaDamageColliders == null || areaDamageColliders.Length == 0)
        {
            areaDamageColliders = AreaDamageGO.GetComponents<BoxCollider2D>();
        }
    }

    public void ResetObj()
    {
        moveIndex = 0;
        gameObject.SetActive(true);
        lifeCurrent = LifeMax;
        state = BossState.Normal;
    }

    public void UpdateObj()
    {

        //REMOVER ISSO DPS
        if (state == BossState.Normal)
            renderer.color = Color.green;
        else if (state == BossState.Stunned)
            renderer.color = Color.blue;
        else if (state == BossState.Recovery)
            renderer.color = Color.red;

        if (bossStunTimer > 0)
        {
            bossStunTimer -= Time.deltaTime;
            currentSpeed = 0;
        }
        else
        {
            if (lifeCurrent > 0)
                currentSpeed = Speed;

            if (state == BossState.Recovery)
            {
                TooglePlataforms(true);
            }

            state = BossState.Normal;
        }


        if (Vector2.Distance(cachedTf.position, nextLocation) < 0.05f)
        {
            SetNextDestination();
        }
        else
        {
            cachedTf.position = Vector2.MoveTowards(cachedTf.position, nextLocation, currentSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(Vector2 damageOrigin, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        if (state == BossState.Normal)
        {
            if (damageSpecialEffect == DamageSpecialEffect.BreakArmor)
            {
                bossStunTimer = BossStunDuration;
                state = BossState.Stunned;
                currentSpeed = 0;
                SoundController.instance.PlayAudioEffect("BossStun");
            }
        }
        else if (state == BossState.Stunned)
        {
            if (damagerType == DamagerType.Player)
            {
                lifeCurrent--;
                state = BossState.Recovery;

                bossStunTimer = RecoveryTime;
                TooglePlataforms(false);

                SoundController.instance.PlayAudioEffect("BossTakeDamage");

                if (lifeCurrent <= 0)
                {
                    GameplayController.instance.FinishGame();
                }
            }
        }
    }

    private void TooglePlataforms(bool toogle)
    {
        for(int i =0; i < Plataforms.Length; i++)
        {
            Plataforms[i].enabled = toogle;
        }

        for(int i = 0; i < areaDamageColliders.Length; i++)
        {
            areaDamageColliders[i].enabled = toogle;
        }
    }

    private enum BossState
    {
        Normal,
        Stunned,
        Recovery,
    }
}
