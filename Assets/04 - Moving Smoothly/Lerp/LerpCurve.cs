using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LerpCurve : MonoBehaviour
{
    [Header("Points")]
    public Transform A;
    public Transform B;

    [Space]
    public AnimationCurve EasingCurve;

    [Header("Time")]
    public FloatController t;
    //[Range(0f,1f)]
    //public float t;

    // Update is called once per frame
    void Update()
    {
        float u = EasingCurve.Evaluate(t.Value);
        transform.position = Vector3.Lerp(A.position, B.position, u);
    }

    void OnDrawGizmos()
    {
        // Line
        Debug.DrawLine(A.position, B.position, Color.red);
        //Debug.DrawLine(A.position, transform.position, Color.blue);
        //Debug.DrawLine(transform.position, B.position, Color.red);

        // Points
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A.position, 0.1f);
        Gizmos.DrawSphere(B.position, 0.1f);

        // Current position
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
