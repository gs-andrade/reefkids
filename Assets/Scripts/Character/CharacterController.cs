using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Character Attributes")]
    public float Speed;
    public float JumpForce;

    [Header("Shoot Knockbak Effect")]
    public float ShootKnockbackGroundForce;
    public float ShootKnockbackAirForce;

    public float HorizontalKnockbackGroundDuration;
    public float HorizontalKnockbackAirDuration;

    [Header("Shoot")]
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    public float ProjectileShootCD;
    public bool EnableProjectile;
    public DamageSpecialEffect DamageSpecialEffect;

    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;
    private float projectileCd;

    private CharacterState state;

    private Vector2 shootKnockbackDirection;
    private float inputDelay;

    public void Setup()
    {
        if (character == null)
        {
            character = GetComponentInChildren<CharacterInstance>(true);
            character.Setup();
        }

        if (input == null)
            input = new PlayerInput();

        state = CharacterState.Normal;
    }

    public void EnableProjectileToBeUsed()
    {
        EnableProjectile = true;
    }
    public CharacterInstance GetPlayer()
    {
        return character;
    }

    public void UpdateCharacters()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;

        if (projectileCd > 0)
            projectileCd -= Time.deltaTime;

        if (character.IsDisabled())
            return;

        input.GetInputs();

        bool isWalking = false;

        switch (state)
        {
            case CharacterState.Normal:
                {
                    var horizontalMOvement = 0f;

                    var grounded = character.CheckIfIsOnGround();

                    if (grounded)
                    {
                        if (wasOnAir)
                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Fall", SoundAction.Play);

                        character.SetAnimationBool("DoubleJ", false);
                        character.SetAnimationBool("IsJumping", false);
                    }

                    wasOnAir = !grounded;
                    // JUMP
                    if (input.JumpPressed && inputDelay <= 0)
                    {
                       // isHoldingJumpButton = true;

                        if (grounded)
                        {
                            character.Jump(JumpForce);
                            character.SetAnimationBool("IsJumping", true);
                            inputDelay = 0.2f;

                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Jump", SoundAction.Play);
                        }
                        else if (EnableProjectile && projectileCd <= 0)
                        {
                            projectileCd = ProjectileShootCD;
                            var direction = new Vector2(0, -1);

                            var projectile = Instantiate(ProjectilePrefab, transform).GetComponent<ProjectileForward>();

                            projectile.Setup((Vector2)character.transform.position + (Vector2.one * direction), direction, ProjectileSpeed, character.gameObject, DamagerType.Player, DamageSpecialEffect);

                            shootKnockbackDirection = ShootKnockbackAirForce * -direction;

                            state = CharacterState.Dashing;

                            character.SetAnimationBool("DoubleJ", true);
                        }
                    }
                    else if (input.Shoot && EnableProjectile && projectileCd <= 0)
                    {
                        /*var inputPositon = Input.mousePosition;
                        inputPositon = Camera.main.ScreenToWorldPoint(inputPositon);*/

                        projectileCd = ProjectileShootCD;

                        var direction = new Vector2(character.transform.localScale.x, 0);

                        var projectile = Instantiate(ProjectilePrefab, transform).GetComponent<ProjectileForward>();

                        projectile.Setup((Vector2)character.transform.position + (Vector2.one * direction), direction, ProjectileSpeed, character.gameObject, DamagerType.Player, DamageSpecialEffect);

                        if (grounded)
                        {
                            shootKnockbackDirection = ShootKnockbackGroundForce * -direction;
                            inputDelay = HorizontalKnockbackGroundDuration;
                            character.SetAnimationBool("ShotG", true);

                        }
                        else
                        {
                            shootKnockbackDirection = ShootKnockbackAirForce * -direction;
                            inputDelay = HorizontalKnockbackAirDuration;
                            character.SetAnimationBool("ShotAir", true);
                        }

                        state = CharacterState.Dashing;

                    }
                    else if (input.Horizontal != 0)
                    {
                        if (grounded)
                            isWalking = true;


                        horizontalMOvement = input.Horizontal * Speed;

                        if (grounded)
                            SoundController.instance.PlayAudioEffect(character.SoundKey + "Step", SoundAction.Play);
                    }

                    character.SetXVelocity(horizontalMOvement);

                    character.SetAnimationBool("IsWalking", isWalking);

                    break;
                }

            case CharacterState.Dashing:
                {
                    //  character.SetMovement(shootKnockbackDirection, false);

                    if (shootKnockbackDirection.y != 0)
                    {
                        character.SetYVelocity(shootKnockbackDirection.y);
                  
                    }
                    else
                    {
                        character.SetXVelocity(shootKnockbackDirection.x, false);
                    }
                       

                    if (inputDelay <= 0)
                    {
                        character.SetAnimationBool("ShotG", false);
                        character.SetAnimationBool("ShotAir", false);

                        state = CharacterState.Normal;
                    }
                    break;
                }
        }


    }


    public void ResetCharacterToStartPosition(Vector2 positionRef)
    {
        character.PowerRelease();
        character.SetMovement(Vector2.zero);
        character.transform.position = positionRef;
    }

    private enum CharacterState
    {
        Normal,
        Dashing,
    }

}
