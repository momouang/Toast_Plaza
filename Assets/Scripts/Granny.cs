using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
    public GameObject[] toastPrefab;
    public Transform shootingPoint;
    //public List<Transform> toasts;

    public float shootingTime = 10f;
    float currentTime;

    private void Start()
    {
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= shootingTime)
        {
            currentTime = 0;
            growToast();
            //Debug.Log("shoot Toast");
        }
    }

    void growToast()
    {
        var toastNOW = Instantiate(toastPrefab[Random.Range(0,3)], shootingPoint.position, Quaternion.identity);
        //toasts.Add(toastNOW.transform);
        toastNOW.GetComponent<Rigidbody>().AddForce(0,0,1000f);
        
    }
}
