using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPoint : MonoBehaviour
{
    public float Radius = 1f;
    public Color Color = Color.yellow;
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position, Radius);
    }

#endif
}
