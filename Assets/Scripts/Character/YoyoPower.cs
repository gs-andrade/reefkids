using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoPower : MonoBehaviour, ICharacterPower
{
    public float VerticalMovementSpeed = 1f;
    private CharacterInstance character;
    private bool canUse;

    private DistanceJoint2D rope;
    private Hook yoyoTarget;
    private InteractiveState hookState;
    private LineRenderer line;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Hook")
            return;

        canUse = true;
        yoyoTarget = collision.GetComponent<Hook>();

        if (yoyoTarget != null)
            yoyoTarget.ToogleArrowRange(true);

    }

    private void Update()
    {
        if (yoyoTarget != null)
            yoyoTarget.ToogleArrowRange(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (yoyoTarget != null)
        {
            if (yoyoTarget.gameObject == collision.gameObject)
            {
                canUse = false;
                yoyoTarget = null;
            }
        }
        else
            canUse = false;
    }
    public bool CanUse()
    {
        return canUse;
    }

    public void Setup()
    {
        character = GetComponentInParent<CharacterInstance>();
        line = GetComponent<LineRenderer>();
        rope = GetComponentInParent<DistanceJoint2D>();
        rope.enabled = false;
    }

    public void Use()
    {
        if (canUse && yoyoTarget != null && hookState == InteractiveState.Unlocked)
        {
            hookState = InteractiveState.Locked;
            rope.connectedBody = yoyoTarget.Rb;
            character.SetGravity(10);
            rope.enabled = true;
           // line.SetPositions
        }
        else if (hookState == InteractiveState.Locked)
        {
            Release();
        }
    }

    public void Release()
    {
        character.SetGravity(1);
        rope.connectedBody = null;
        rope.enabled = false;
        hookState = InteractiveState.Unlocked;
    }
}
