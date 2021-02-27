using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueTriggerType TriggerType = DialogueTriggerType.Normal;
    private bool trigger = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.gameObject.GetComponent<CharacterInstance>();

        if (character != null && !trigger)
        {
            if (TriggerType == DialogueTriggerType.ForceEnd)
                GameplayController.instance.DisableDialogue();
            else
            {
                GameplayController.instance.ActiveDialogue();
                trigger = true;
            }
        }

    }

    public enum DialogueTriggerType
    {
        Normal,
        ForceEnd,
    }
}
