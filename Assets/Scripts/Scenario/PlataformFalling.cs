using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformFalling : MonoBehaviour
{
    public float TimeBeforeFall;
    public float FallSpeed;


    private Rigidbody2D rb;
    private InteractiveState state;
    private float startYPos;
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
            if(TimeBeforeFall > 0)
            {
                TimeBeforeFall -= Time.deltaTime;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = FallSpeed;
            }
        }
    }
}
