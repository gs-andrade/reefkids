using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance : MonoBehaviour,  IDamagable
{

    [Header("Ground Collision Checks")]
    public Vector2 RaycastFootOffset;
    public float RayCastFootLenght;
    public LayerMask GroundLayer;

    [Header("SoundEffect")]
    public string SoundKey;

    private Transform cachedTf;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private Animator animator;

    private ICharacterPower characterPower;
    private float disableTime;

    public void Setup()
    {
        cachedTf = transform;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        if (characterPower == null)
            characterPower = GetComponentInChildren<ICharacterPower>();

        if (characterPower != null)
            characterPower.Setup();

    }

    public bool CheckIfIsOnGround()
    {
        collider.enabled = false;

        var leftCheck = CollisionUtils.CustomRaycast(cachedTf.position, -RaycastFootOffset, Vector2.down, RayCastFootLenght, GroundLayer, true);
        var rightCheck = CollisionUtils.CustomRaycast(cachedTf.position, RaycastFootOffset, Vector2.down, RayCastFootLenght, GroundLayer, true);
        var centerCheck = CollisionUtils.CustomRaycast(cachedTf.position, Vector2.zero, Vector2.down, RayCastFootLenght, GroundLayer, true);

        collider.enabled = true;

        if (leftCheck || rightCheck || centerCheck)
        {
            animator.SetBool("IsJumping", true);
            return true;
        }
        else
        {
            animator.SetBool("IsJumping", false);
            return false;
        }
    }

    public bool IsDisabled()
    {
        if (disableTime > 0)
            return true;

        return false;
    }

    public void TakeDamage(Vector2 damageOrigin, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None, int ammount = 1)
    {
        var direction = (new Vector2(cachedTf.position.x - damageOrigin.x, 1)).normalized;

        disableTime = 1f;

        SoundController.instance.PlayAudioEffect("Damage");

        if (GameplayController.instance.TakeDamageAndCheckIfIsAlive(ammount))
            SetMovement(direction * new Vector2(5,10) );
    }

    public void SetGravity(float ammount)
    {
        rb.gravityScale = ammount;
    }

    public void SetMovement(Vector2 movement)
    {
        rb.velocity = movement;

        if (movement != Vector2.zero)
        {
            if (movement.x > 0)
                cachedTf.localScale = new Vector3(1, 1, 1);
            else if (movement.x < 0)
                cachedTf.localScale = new Vector3(-1, 1, 1);

            animator.SetBool("IsWalking", false);
        }
    }

    public float GetDirection()
    {
        return cachedTf.localScale.x;
    }

    public void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
        animator.SetBool("IsJumping", true);
    }


    public void SetXVelocity(float xMove)
    {
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        if (xMove != 0)
        {
            if (xMove > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    public void PowerUser()
    {
        if (characterPower != null)
        {
            characterPower.Use();
        }
    }

    public void PowerRelease()
    {
        if (characterPower != null)
        {
            characterPower.Release();
        }
    }

    private void FixedUpdate()
    {
        if (disableTime > 0)
            disableTime -= Time.deltaTime;
    }


}
