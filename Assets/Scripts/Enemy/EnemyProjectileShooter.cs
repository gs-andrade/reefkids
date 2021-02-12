using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileShooter : EnemyGeneric
{
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed = 25f;
    public float DelayToShoot = 2f;


    private float delayTimer;
    private Transform projSight;
    private Transform cachedTf;



    public override void ResetObj()
    {

    }

    public override void SetupOnStartLevel()
    {
        if (projSight == null)
            projSight = GetComponentInChildren<DrawPoint>().transform;

        delayTimer = DelayToShoot;
        cachedTf = transform;
    }


    public override void UpdateObj()
    {
        if (projSight == null)
            return;

        if (disableTime > 0)
        {
            disableTime -= Time.deltaTime;
            return;
        }


        if (delayTimer > 0)
            delayTimer -= Time.deltaTime;
        else
        {
            delayTimer = DelayToShoot;
            var direction = (projSight.position - cachedTf.position).normalized;

            var projectile = Instantiate(ProjectilePrefab, transform).GetComponent<ProjectileForward>();

            projectile.Setup((Vector2)cachedTf.position + (Vector2.one * direction), direction, ProjectileSpeed, gameObject);
        }
    }
}
