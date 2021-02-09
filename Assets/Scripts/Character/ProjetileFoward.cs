using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetileFoward : MonoBehaviour, IUpdatable
{
    private Transform cachedTf;
    private Vector2 direction;
    private float speed;
 
    public void Setup(Vector2 startPosition, Vector2 direction, float speed)
    {
        if (cachedTf == null)
            cachedTf = transform;

        cachedTf.transform.position = startPosition;
        this.direction = direction;
        this.speed = speed;
    }

    public void UpdateObj()
    {
        cachedTf.Translate(direction * speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
