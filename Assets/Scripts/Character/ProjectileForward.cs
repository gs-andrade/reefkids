using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForward : MonoBehaviour, IDamagable  //, IUpdatable
{
    public float MaxAliveTime = 5f;
    public GameObject FakeStunnedProjectile;
    private Transform cachedTf;
    private Vector2 direction;
    private float speed;
    private GameObject owner;
    private float aliveTime;
    private DamagerType damagerType;
    private DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None;
    public void Setup(Vector2 startPosition, Vector2 direction, float speed, GameObject owner, DamagerType projectileOwnerType, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        if (cachedTf == null)
            cachedTf = transform;

        cachedTf.transform.position = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.owner = owner;
        damagerType = projectileOwnerType;
        this.damageSpecialEffect = damageSpecialEffect;
    }

    public DamagerType GetProjectileOwnerType()
    {
        return damagerType;
    }

    private void Update()
    {
        if (cachedTf == null)
            cachedTf = transform;

        cachedTf.Translate(direction * speed * Time.deltaTime);

        aliveTime += Time.deltaTime;

        if (aliveTime > MaxAliveTime)
            Destroy(gameObject);
    }

    // dps quando vcc for uusar pool
    /*  public void UpdateObj()
      {
          cachedTf.Translate(direction * speed * Time.deltaTime);
      }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != owner)
        {
            var target = collision.gameObject.GetComponent<IDamagable>();

            if (target != null)
                target.TakeDamage(cachedTf.position, damagerType, 1, damageSpecialEffect);

            Destroy(gameObject);
        }
    }

    public void TakeDamage(Vector2 damagerPosition, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        if (damageSpecialEffect == DamageSpecialEffect.Stun)
        {
            var fakeProject = Instantiate(FakeStunnedProjectile, transform.parent);
            fakeProject.transform.position = transform.position;
        }

    }
}

public enum DamagerType
{
    Enemy,
    Player,
}