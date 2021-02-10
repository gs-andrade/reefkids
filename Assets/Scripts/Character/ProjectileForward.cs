using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileForward : MonoBehaviour//, IUpdatable
{
    public float MaxAliveTime = 5f;
    private Transform cachedTf;
    private Vector2 direction;
    private float speed;
    private GameObject owner;
    private float aliveTime;
    public void Setup(Vector2 startPosition, Vector2 direction, float speed, GameObject owner)
    {
        if (cachedTf == null)
            cachedTf = transform;

        cachedTf.transform.position = startPosition;
        this.direction = direction;
        this.speed = speed;
        this.owner = owner;
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
                target.TakeDamage(cachedTf.position, DamageSpecialEffect.Stun, 0);

            Destroy(gameObject);
        }
    }
}
