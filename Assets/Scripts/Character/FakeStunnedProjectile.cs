using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeStunnedProjectile : MonoBehaviour
{
    public float AliveTime = 5f;

    private void FixedUpdate()
    {
        if (AliveTime >= 0)
            AliveTime -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

}
