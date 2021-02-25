using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public ParticleSystem dust;
    public ParticleSystem landingdust;
    public Vector2 LandingDustOffset;
    
    private bool spawnDust;

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
    public float DElayTime;

    private CharacterInstance character;

    private PlayerInput input;

    private bool wasOnAir = true;
    private float projectileCd;

    private CharacterState state;

    private Vector2 shootKnockbackDirection;
    private float inputDelay;

    private float coyoteJump;


    private int soundCount = 0;


    private float soundCharacterDelayToUseagain;
    private float soundCharacterDelayToReseTimer;


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

    private void PlayCharacterSound(string name)
    {
        if (soundCharacterDelayToReseTimer > 0)
            return;

        if (soundCount >= 5)
            soundCharacterDelayToReseTimer = 3.5f;

        soundCharacterDelayToUseagain = 5f;
        soundCount++;
        SoundController.instance.PlayAudioEffect(name);
    }
    private void PlayGenericSound(string name)
    {
        SoundController.instance.PlayAudioEffect(name);
    }
    private void UpdateSoundCount()
    {
        if (soundCharacterDelayToUseagain > 0)
        {
            soundCharacterDelayToUseagain -= Time.deltaTime;

            if(soundCharacterDelayToUseagain <= 0)
            {
                soundCount = 0;
            }
        }

        if (soundCharacterDelayToReseTimer > 0)
        {
            soundCharacterDelayToReseTimer -= Time.deltaTime;
            if (soundCharacterDelayToReseTimer <= 0)
            {
                soundCount = 0;
            }
        }
    }
    public void UpdateCharacters()
    {
        if (inputDelay > 0)
            inputDelay -= Time.deltaTime;

        if (projectileCd > 0)
            projectileCd -= Time.deltaTime;

        if (coyoteJump > 0)
            coyoteJump -= Time.deltaTime;

        if (character.IsDisabled())
            return;

        UpdateSoundCount();

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
                        if (spawnDust == true)
                        {
                            CreateLandingdust();
                            spawnDust = false;
                        }

                        coyoteJump = 0.25f;

                        if (wasOnAir)
                            PlayGenericSound("Fall");

                        character.SetAnimationBool("DoubleJ", false);
                        character.SetAnimationBool("IsJumping", false);
                    }
                    else
                    {
                        spawnDust = true;
                        character.SetAnimationBool("IsJumping", true);
                    }

                       

                    wasOnAir = !grounded;
                    // JUMP
                    if (input.JumpPressed && inputDelay <= 0)
                    {
                        // isHoldingJumpButton = true;

                        if (grounded || coyoteJump > 0)
                        {
                            CreateDust();
                            character.Jump(JumpForce);
                            inputDelay = 0.2f;
                            coyoteJump = 0;

                         

                            PlayGenericSound("Jump");
                            PlayCharacterSound("jump1");

                            soundCount++;
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

                            PlayCharacterSound("jump2");

                            soundCount++;
                        }
                    }
                    else if (input.Shoot && EnableProjectile && projectileCd <= 0)
                    {
                        /*var inputPositon = Input.mousePosition;
                        inputPositon = Camera.main.ScreenToWorldPoint(inputPositon);*/

                        projectileCd = ProjectileShootCD;

                        var direction = new Vector2(character.transform.localScale.x, 0);

                        var projectile = Instantiate(ProjectilePrefab, transform).GetComponent<ProjectileForward>();

                        if (grounded)
                            projectile.SetDelayToAppe(DElayTime);

                        projectile.Setup((Vector2)character.transform.position + (Vector2.one * direction), direction, ProjectileSpeed, character.gameObject, DamagerType.Player, DamageSpecialEffect);

                        if (grounded)
                        {
                            shootKnockbackDirection = ShootKnockbackGroundForce * -direction;
                            inputDelay = HorizontalKnockbackGroundDuration;
                            character.SetAnimationBool("ShotG", true);
                            PlayCharacterSound("attack1");

                        }
                        else
                        {
                            shootKnockbackDirection = ShootKnockbackAirForce * -direction;
                            inputDelay = HorizontalKnockbackAirDuration;
                            character.SetAnimationBool("ShotAir", true);
                            PlayCharacterSound("ice");
                        }

                        state = CharacterState.Dashing;

                    }
                    else if (input.Horizontal != 0)
                    {
                        if (grounded)
                            isWalking = true;


                        horizontalMOvement = input.Horizontal * Speed;

                        if (grounded)
                            PlayGenericSound("Step");
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

    void CreateDust()
    {
        dust.Play();
    }

    void CreateLandingdust()
    {
        landingdust.transform.position = (Vector2)character.transform.position + LandingDustOffset;
        landingdust.Play();
    }
}
