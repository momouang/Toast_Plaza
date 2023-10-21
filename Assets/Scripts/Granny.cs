using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
    public GameObject[] toastPrefab;
    public Transform shootingPoint;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            growToast();
        }
    }

    void growToast()
    {
        var toastNOW = Instantiate(toastPrefab[0], shootingPoint.position, Quaternion.identity);
        toastNOW.GetComponent<Rigidbody>().AddForce(0,0,1000f);
        
    }
}
