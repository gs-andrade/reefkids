using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformFalling : MonoBehaviour, IInterctable, IUpdatable, IDamagable
{
    public float TimeBeforeFall;
    public float FallSpeed;
    public bool Damageble;

    private float timerBeforeFall;
    private Rigidbody2D rb;
    private InteractiveState state;
    private Vector2 startPosition;


    private bool soundPlayed;
    public void ResetObj()
    {
        if (rb != null)
            rb.gravityScale = 0;

        state = InteractiveState.Locked;
        transform.position = startPosition;
        timerBeforeFall = 0;
        soundPlayed = false;
    }

    public void SetupOnStartLevel()
    {
        startPosition = transform.position;
        timerBeforeFall = 0;
    }

    public void TakeDamage(Vector2 damageOrigin, DamagerType damagerType, int ammount = 1, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None)
    {
        if (Damageble && damagerType == DamagerType.Player)
        {
            SoundController.instance.PlayAudioEffect("PlatOnHit", SoundAction.Play);
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
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Damageble)
            return;

        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null &&  (character.transform.position.y - transform.position.y > 0))
        {
            Fall();
        }
        else
        {
            if(state == InteractiveState.Unlocked)
            {
                SoundController.instance.PlayAudioEffect("PlatOnFall", SoundAction.Play);
                soundPlayed = true;
            }
        }
        
    }

    private void Fall()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        state = InteractiveState.Unlocked;
    }

}
