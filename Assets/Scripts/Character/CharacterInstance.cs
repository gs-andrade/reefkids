using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInstance : MonoBehaviour, IUpdatable
{

    [Header("Ground Collision Checks")]
    public Vector2 RaycastFootOffset;
    public float RayCastFootLenght;
    public LayerMask GroundLayer;

    public CharacterType CharacterType;
    public float Speed;
    public float JumpForce;

    [Header("SoundEffect")]
    public string SoundKey;

    private Transform cachedTf;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private Animator animator;

    private ICharacterPower characterPower;
    private float disableTime;

    private Vector3 savedVelocity;
    private float savedAngularVelocity;
    private RigidbodyConstraints2D savedConstraints;

    public void Setup()
    {
        cachedTf = transform;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        if (characterPower == null)
            characterPower = GetComponentInChildren<ICharacterPower>();


        if (characterPower != null)
        {
            characterPower.Setup();
        }

        GameplayController.instance.RegisterPause(OnPauseGame);
        GameplayController.instance.RegisterUnpause(OnResumeGame);

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

    public void LockkMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void UnlockMovement()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void TakeDamage(Vector2 origin, Vector2 force, int ammount = 1)
    {
        var direction = (new Vector2(cachedTf.position.x - origin.x, 1)).normalized;

        disableTime = 1f;

        SoundController.instance.PlayAudioEffect("Damage");

        if (GameplayController.instance.TakeDamageAndCheckIfIsAlive(ammount))
            SetMovement(direction * force);
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
                transform.localScale = new Vector3(1, 1, 1);
            else if (movement.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            animator.SetBool("IsWalking", false);
        }
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        animator.SetBool("IsJumping", true);
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
        if(characterPower != null)
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

    public void UpdateObj()
    {
        disableTime -= Time.deltaTime;

        switch (CharacterType)
        {
            case CharacterType.Ranged:
                {


                    break;
                }
        }
    }

    private void OnPauseGame()
    {
        savedVelocity = rb.velocity;
        savedAngularVelocity = rb.angularVelocity;
        savedConstraints = rb.constraints;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnResumeGame()
    {
        rb.velocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
        rb.constraints = savedConstraints;
    }
}

public enum CharacterType
{
    Strong,
    Ranged,
    Tiny
}
