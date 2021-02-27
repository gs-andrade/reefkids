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
    private Animator animator;
    private Transform spriteTf;


    public override void ResetObj()
    {

    }

    public override void SetupOnStartLevel()
    {
        delayTimer = DelayToShoot;
        cachedTf = transform;

        if (projSight == null)
            projSight = GetComponentInChildren<DrawPoint>(true).transform;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(true);
            spriteTf = animator.transform;

            var direction = (projSight.position - cachedTf.position).normalized;

            if (direction.x < 0)
                spriteTf.localScale = new Vector3(-1, 1, 1);
            else if (direction.y > 0)
                spriteTf.rotation = new Quaternion(0, 0, 90, 1);
            else if (direction.y < 0)
                spriteTf.rotation = new Quaternion(0, 0, 270, 1);
        }

       
    }

    public override void UpdateObj()
    {

        animator.SetBool("Shoot", false);

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

            projectile.Setup((Vector2)cachedTf.position + (Vector2.one * direction), direction, ProjectileSpeed, gameObject, DamagerType.Enemy);
            animator.SetBool("Shoot", true);
        }
    }
}

