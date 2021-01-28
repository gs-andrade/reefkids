﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance : MonoBehaviour
{

    [Header("Ground Collision Checks")]
    public Vector2 RaycastFootOffset;
    public float RayCastFootLenght;
    public LayerMask GroundLayer;


    public CharacterType CharacterType;
    public float Speed;
    public float JumpForce;

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

    private void LateUpdate()
    {
        CheckIfIsOnGround();
    }
    public bool CheckIfIsOnGround()
    {
        collider.enabled = false;

        var leftCheck = CollisionUtils.CustomRaycast(cachedTf.position, -RaycastFootOffset, Vector2.down, RayCastFootLenght, GroundLayer, true);
        var rightCheck = CollisionUtils.CustomRaycast(cachedTf.position, RaycastFootOffset, Vector2.down, RayCastFootLenght, GroundLayer, true);
        var centerCheck = CollisionUtils.CustomRaycast(cachedTf.position, Vector2.zero, Vector2.down, RayCastFootLenght, GroundLayer, true);

        collider.enabled = true;

        if (leftCheck || rightCheck)
            return true;
        else
            return false;
    }

    public void LockkMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
    }

    public void SetXVelocity(float xMove)
    {
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        if (xMove != 0)
            lastFacingDirection = rb.velocity.normalized;
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
