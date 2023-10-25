using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    [Range(0,10)]
    public float SmoothTime;
    private Vector2 Velocity;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, Target.position, ref Velocity, SmoothTime);
    }
}
