using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance : MonoBehaviour
{

    [Header("Ground Collision Checks")]
    public Vector2 RaycastFootOffset;
    public float RayCastFootLenght;
    public LayerMask GroundLayer;

    public float DisableTime = 1f;
    public Vector2 KnocbakcForce;

  

    [Header("SoundEffect")]
    public string SoundKey;

    private Transform cachedTf;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer renderer;

    private ICharacterPower characterPower;
    private float disableTime;

    public void Setup()
    {
        cachedTf = transform;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        renderer = GetComponent<SpriteRenderer>();

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
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsDisabled()
    {
        if (disableTime > 0)
        { 
            return true;
        }

        return false;
    }

    public bool TakeDamage(Vector2 damageOrigin)
    {

        if (disableTime > 0)
            return false;

        var dirX = cachedTf.position.x - damageOrigin.x >= 0 ? 1 : -1;
        var direction = (new Vector2(dirX, 1)).normalized;

        disableTime = DisableTime;

        SoundController.instance.PlayAudioEffect("Damage");

        SetMovement(direction * new Vector2(KnocbakcForce.x * dirX , KnocbakcForce.y), false);

        return true;
    }

    public void SetGravity(float ammount)
    {
        rb.gravityScale = ammount;
    }

    public void SetMovement(Vector2 movement, bool changeDirection = true)
    {
        rb.velocity = movement;

        if (movement != Vector2.zero)
        {
            if (changeDirection)
            {
                if (movement.x > 0)
                    cachedTf.localScale = new Vector3(1, 1, 1);
                else if (movement.x < 0)
                    cachedTf.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public float GetDirection()
    {
        return cachedTf.localScale.x;
    }

    public void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }


    public void SetXVelocity(float xMove, bool changeDirection = true)
    {
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        if (xMove != 0)
        {
            if (changeDirection)
            {
                if (xMove > 0)
                    transform.localScale = new Vector3(1, 1, 1);
                else
                    transform.localScale = new Vector3(-1, 1, 1);
            }
        }

    }

    public void SetYVelocity(float yMOve)
    {
        rb.velocity = new Vector2(rb.velocity.x, yMOve);
    }

    public void SetAnimationTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public void SetAnimationBool(string name, bool state)
    {
        animator.SetBool(name, state);
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
        {
            disableTime -= Time.deltaTime;
            renderer.enabled = !renderer.enabled;
        }
        else
        {
            renderer.enabled = true;
        }
    }
}
