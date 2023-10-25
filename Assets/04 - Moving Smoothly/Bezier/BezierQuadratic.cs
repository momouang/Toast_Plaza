using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierQuadratic : MonoBehaviour
{
    [Header("Points")]
    public Transform A;
    public Transform B;

    [Header("Control Point")]
    public Transform C;

    [Header("Time")]
    public FloatController t;
    //[Range(0f,1f)]
    //public float t;

    // Update is called once per frame
    void Update()
    {
        transform.position = Lerp(t.Value);
    }

    // Used to lerp
    public Vector3 Lerp (float t)
    {
        Vector3 ac = Vector3.Lerp(A.position, C.position, t);
        Vector3 cb = Vector3.Lerp(C.position, B.position, t);
        Vector3 p = Vector3.Lerp(ac, cb, t);
        return p;
    }

    void OnDrawGizmos()
    {
        // Spheres A, B
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A.position, 0.1f);
        Gizmos.DrawSphere(B.position, 0.1f);


        // Lerp on the segment AC
        Debug.DrawLine(A.position, C.position, Color.red);

        Vector3 AC = Vector3.Lerp(A.position, C.position, t.Value);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(AC, 0.1f);

        // Lerp on the segment CB
        Debug.DrawLine(C.position, B.position, Color.red);

        Vector3 CB = Vector3.Lerp(C.position, B.position, t.Value);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(CB, 0.1f);

        // Lerp on the segment AC-CB
        Debug.DrawLine(AC, CB, Color.blue);

        Vector3 P = Vector3.Lerp(AC, CB, t.Value);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(P, 0.1f);




        // Draws the entire curve
        float dt = 0.1f;
        for (float t = 0; t < 1; t += dt)
        {
            Debug.DrawLine
            (
                Lerp(t), Lerp(t+dt),
                Color.white
            );
        }
    }
}
