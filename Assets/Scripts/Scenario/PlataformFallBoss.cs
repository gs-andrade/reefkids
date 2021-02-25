using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformFallBoss : MonoBehaviour, IInterctable, IUpdatable, IDamagable
{
    public float TimeBeforeFall;
    public float FallSpeed;
    public float TimeToRespawn;

    public GameObject[] Shields;

    private bool leftOrRight;

    private float timerToRespawn;
    private float timerBeforeFall;
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private SpriteRenderer renderer;
    private InteractiveState state;
    private Vector2 startPosition;


    private bool soundPlayed;
    public void ResetObj()
    {

        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        state = InteractiveState.Locked;
        transform.position = startPosition;
        timerBeforeFall = TimeBeforeFall;

        collider.enabled = true;
        renderer.enabled = true;

        ToogleShields();
    }

    public void SetupOnStartLevel()
    {
        startPosition = transform.position;
        timerBeforeFall = TimeBeforeFall;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (collider == null)
            collider = GetComponent<BoxCollider2D>();

        if (renderer == null)
            renderer = GetComponent<SpriteRenderer>();
    }

    public void ToogleShields()
    {
        Shields[0].SetActive(leftOrRight);
        Shields[1].SetActive(!leftOrRight);

        leftOrRight = !leftOrRight;
    }

    public void TakeDamage(Vector2 damageOrigin, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        if (damagerType == DamagerType.Player)
        {
            Fall();
        }
    }

    public void UpdateObj()
    {
        if (state == InteractiveState.Unlocked)
        {
            if (timerBeforeFall > 0)
            {
                timerBeforeFall -= Time.deltaTime;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = FallSpeed;

                if (!soundPlayed)
                {
                    SoundController.instance.PlayAudioEffect("PlatOnHit");
                    soundPlayed = true;
                }

                if (timerToRespawn > 0)
                {
                    timerToRespawn -= Time.deltaTime;
                }
                else
                    ResetObj();
            }
        }
        else
            timerToRespawn = TimeToRespawn;

    }

  /*  private void OnCollisionEnter2D(Collision2D collision)
    {
        if(state == InteractiveState.Unlocked)
        {
            var damageble = collision.gameObject.GetComponent<IDamagable>();

            if (damageble != null)
                damageble.TakeDamage(transform.position, DamagerType.Player, 1, DamageSpecialEffect.BreakArmor);

            DisableObject();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == InteractiveState.Unlocked)
        {
            if (collision.tag != "Boss")
                return;

            var damageble = collision.gameObject.GetComponent<IDamagable>();

            if (damageble != null)
                damageble.TakeDamage(transform.position, DamagerType.Player, 1, DamageSpecialEffect.BreakArmor);


            DisableObject();
        }
    }

    private void DisableObject()
    {
        SoundController.instance.PlayAudioEffect("PlatOnFall");
        collider.enabled = false;
        renderer.enabled = false;

        Shields[0].SetActive(false);
        Shields[1].SetActive(false);
    }

    private void Fall()
    {
        state = InteractiveState.Unlocked;
    }
}
