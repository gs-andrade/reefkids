using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformSeal : MonoBehaviour
{
    public float StrongJumpForce;
    public float RangedJumpForce;
    public float TinyJumpForce;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if(character.transform.position.y - transform.position.y > 0)
            {
                float jumpForce = 0;
                switch (character.CharacterType)
                {
                    case CharacterType.Strong:
                        {
                            jumpForce = StrongJumpForce;
                            break;
                        }

                    case CharacterType.Ranged:
                        {
                            jumpForce = RangedJumpForce;
                            break;
                        }

                        case CharacterType.Tiny:
                        {
                            jumpForce = TinyJumpForce;
                            break;
                        }
                }

                SoundController.instance.PlayAudioEffect("PlataformSeal");

                character.Jump(jumpForce);
            }
            
        }
    }
}
