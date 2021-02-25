using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    public List<MovingWall> MovingWall;

    public float Speed;
    public float DistanceToReset;
    private void Awake()
    {
        for(int i = 0; i < MovingWall.Count; i++)
        {
            MovingWall[i].Setup();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < MovingWall.Count; i++)
        {
            MovingWall[i].UpdatePosition(Speed, DistanceToReset);
        }
    }
}



[System.Serializable]
public class MovingWall
{
    public Transform Tf;
    private Vector2 startPosition;

    public void Setup()
    {
        startPosition = Tf.position;
    }

    public void UpdatePosition(float speed, float maxDistance)
    {
        Tf.Translate(new Vector2(0, speed * Time.deltaTime));

        if(Vector2.Distance(Tf.position, startPosition) > maxDistance)
        {
            Tf.position = startPosition;
        }
    }
}