using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 offset;
	public float smoothSpeed = 0.125f;


	void FixedUpdate()
	{
		Vector3 desiredPosition = (Vector2)player.position + offset;
		Vector3 smoothedPosition = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);

	}
}
