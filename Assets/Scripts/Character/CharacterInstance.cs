using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance : MonoBehaviour
{
    public CharacterType CharacterType;
    public float castDistance;

    private Transform cachedTf;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private Vector2 lastFacingDirection;

    public void Setup()
    {
        cachedTf = transform;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void LockkMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void UnlockMovement()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetMovement(Vector2 movement)
    {
        rb.velocity = movement;

        if (movement != Vector2.zero)
            lastFacingDirection = movement.normalized;
    }

    public void UseSkill()
    {
        switch (CharacterType)
        {
            case CharacterType.Ranged:
                {
                    collider.enabled = false;
                    var hit = Physics2D.Raycast(cachedTf.position, lastFacingDirection, 3f);
                    collider.enabled = true;

                    if (hit)
                        Debug.Log(hit.collider.gameObject.name);

                    break;
                }

            case CharacterType.Tiny:
                {
                    transform.localScale = Vector2.one / 2;
                    break;
                }
        }
    }
}


public enum CharacterType
{
    Strong,
    Ranged,
    Tiny
}
