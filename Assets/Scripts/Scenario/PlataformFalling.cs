using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformFalling : MonoBehaviour, IInterctable
{
    public float TimeBeforeFall;
    public float FallSpeed;

    private float timerBeforeFall;
    private Rigidbody2D rb;
    private InteractiveState state;
    private float startYPos;
    private Vector2 startPosition;

    public void Reset()
    {
        if (rb != null)
            rb.gravityScale = 0;

        state = InteractiveState.Locked;
        transform.position = startPosition;
        timerBeforeFall = TimeBeforeFall;
    }

    public void SaveStart()
    {
        startPosition = transform.position;
        timerBeforeFall = TimeBeforeFall;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if (rb == null)
                rb = GetComponent<Rigidbody2D>();

            startYPos = transform.position.y;

            state = InteractiveState.Unlocked;
        }
    }

    private void Update()
    {
        if(state == InteractiveState.Unlocked)
        {
            if(timerBeforeFall > 0)
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
}
