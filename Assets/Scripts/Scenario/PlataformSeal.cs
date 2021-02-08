using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformSeal : MonoBehaviour
{
    public float JumpForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null)
        {
            if(character.transform.position.y - transform.position.y > 0)
            {

                SoundController.instance.PlayAudioEffect("PlataformSeal");

                character.Jump(JumpForce);
            }
            
        }
    }
}
