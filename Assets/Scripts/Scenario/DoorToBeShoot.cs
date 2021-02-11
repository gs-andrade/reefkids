using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorToBeShoot : MonoBehaviour, IInterctable, IUpdatable, IDamagable
{
    public float OpeningSpeed = 5;
    private InteractiveState state;
    private Vector2 drawPoint;
    public void ResetObj()
    {
       
    }

    public void SetupOnStartLevel()
    {
        state = InteractiveState.Locked;
        drawPoint = GetComponentInChildren<DrawPoint>().transform.position;
    }

    public void TakeDamage(Vector2 damageOrigin, DamageSpecialEffect damageSpecialEffect = DamageSpecialEffect.None, int ammount = 1)
    {
        state = InteractiveState.Unlocked;
    }

    public void UpdateObj()
    {
        if(state == InteractiveState.Unlocked)
        {
            transform.position = Vector2.MoveTowards(transform.position, drawPoint, OpeningSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, drawPoint) <= 0.05f)
            {
                state = InteractiveState.Locked;
            }
        }
    }
}
