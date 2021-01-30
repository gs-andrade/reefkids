using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public Color Color = Color.yellow;
    public bool WireMode;
    private CircleCollider2D collider;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (collider == null)
            collider = GetComponent<CircleCollider2D>();

        Gizmos.color = Color;

        if (WireMode)
            Gizmos.DrawWireSphere(transform.position, collider.radius);
        else
            Gizmos.DrawSphere(transform.position, collider.radius);
    }

#endif
}
