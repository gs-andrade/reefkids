using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    public static PoolController instance { private set; get; }

    public ProjectileForward GenericProjectile;

    public void Awake()
    {
        instance = this;
    }



 

}
