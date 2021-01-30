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
    private Vector2 lastFacingDirection;

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
            return true;
        else
            return false;
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
            lastFacingDirection = movement.normalized;
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
    }

    public void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
    }

    public void SetXVelocity(float xMove)
    {
        rb.velocity = new Vector2(xMove, rb.velocity.y);

        if (xMove != 0)
            lastFacingDirection = rb.velocity.normalized;
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
