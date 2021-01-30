using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorButton : MonoBehaviour, IInterctable, IUpdatable
{
    public Transform Door;
    private BoxCollider2D collider;
    private InteractiveState boxState;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collider == null)
            collider = GetComponent<BoxCollider2D>();

        boxState = InteractiveState.Unlocked;

        SoundController.instance.PlayAudioEffect("DoorOpen", SoundAction.Play);

    }

    public void Reset()
    {
        Door.gameObject.SetActive(true);
        boxState = InteractiveState.Locked;
    }

    public void SaveStart()
    {
     
    }

    public void UpdateObj()
    {
        if (boxState == InteractiveState.Unlocked)
        {
            collider.enabled = false;
            var hit = Physics2D.BoxCast(transform.position, Vector2.one, 0, Vector2.zero);
            collider.enabled = true;

            if (!hit)
                boxState = InteractiveState.Locked;

            Door.gameObject.SetActive(false);
        }
        else
        {
            Door.gameObject.SetActive(true);
        }
    }

}
