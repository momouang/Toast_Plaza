using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierCubic : MonoBehaviour
{
    [Header("Points")]
    public Transform A;
    public Transform B;

    [Header("Control Point")]
    public Transform C1;
    public Transform C2;

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
        Vector3 ac1  = Vector3.Lerp(A .position, C1.position, t);
        Vector3 c1c2 = Vector3.Lerp(C1.position, C2.position, t);
        Vector3 c2b  = Vector3.Lerp(C2.position, B .position, t);

        Vector3 ac1_c1c2 = Vector3.Lerp(ac1, c1c2, t);
        Vector3 c1c2_c2b = Vector3.Lerp(c1c2, c2b, t);

        Vector3 p = Vector3.Lerp(ac1_c1c2, c1c2_c2b, t);
        return p;
    }

    
    void OnDrawGizmos()
    {
        // Spheres A, B
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A.position, 0.1f);
        Gizmos.DrawSphere(B.position, 0.1f);

        // Spheres C1, C2
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(A.position, 0.1f);
        //Gizmos.DrawSphere(B.position, 0.1f);


        // Lerp on the segment AC1
        Debug.DrawLine(A.position, C1.position, Color.red);

        Vector3 AC1 = Vector3.Lerp(A.position, C1.position, t.Value);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(AC1, 0.1f);

        // Lerp on the segment C1C2
        Debug.DrawLine(C1.position, C2.position, Color.red);

        Vector3 C1C2 = Vector3.Lerp(C1.position, C2.position, t.Value);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(C1C2, 0.1f);

        // Lerp on the segment C2B
        Debug.DrawLine(C2.position, B.position, Color.red);

        Vector3 C2B = Vector3.Lerp(C2.position, B.position, t.Value);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(C2B, 0.1f);




        // Lerp on the segment AC1-C1C2
        Debug.DrawLine(AC1, C1C2, Color.blue);

        Vector3 AC1_C1C2 = Vector3.Lerp(AC1, C1C2, t.Value);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(AC1_C1C2, 0.1f);

        // Lerp on the segment C1C2-C2B
        Debug.DrawLine(C1C2, C2B, Color.blue);

        Vector3 C1C2_C2B = Vector3.Lerp(C1C2, C2B, t.Value);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(C1C2_C2B, 0.1f);




        // Lerp on the final segment
        Debug.DrawLine(AC1_C1C2, C1C2_C2B, Color.green);

        Vector3 P = Vector3.Lerp(AC1_C1C2, C1C2_C2B, t.Value);
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
