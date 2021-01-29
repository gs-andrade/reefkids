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

    private Transform cachedTf;
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private Vector2 lastFacingDirection;

    private float disableTime;


    private Vector3 savedVelocity;
    private float savedAngularVelocity;
    private RigidbodyConstraints2D savedConstraints;


    public void Setup()
    {
        cachedTf = transform;
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();


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

        if (GameplayController.instance.TakeDamageAndCheckIfIsAlive(ammount))
            SetMovement(direction * force);
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

    public void UpdateObj()
    {
        disableTime -= Time.deltaTime;
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
