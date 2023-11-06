using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Granny : MonoBehaviour
{
    [Header("Shooting System")]
    public GameObject[] toastPrefab;
    public Transform shootingPoint;
    public float shootingTime = 10f;
    float currentTime;
    public ParticleSystem Fire;

    [Header("Patrolling System")]
    private NavMeshAgent grannyAi;
    public Vector3 walkpoint;
    bool walkpointSet;
    public float walkpointRange;
    public Transform lookingPoint;
    public Transform backRPG;


    private void Start()
    {
        currentTime = 0;
        grannyAi = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime >= shootingTime)
        {
            currentTime = 0;
            growToast();
        }

        patrolling();

    }


    private void patrolling()
    {
        if (!walkpointSet)
        {
            searchwalkPoint();
        }

        if (walkpointSet)
        {
            grannyAi.SetDestination(walkpoint);
        }

        Vector3 distancetowalkPoint = transform.position - walkpoint;

        if (distancetowalkPoint.magnitude < 1f)
        {
            walkpointSet = false;
        }

        gameObject.transform.LookAt(lookingPoint);
    }

    private void searchwalkPoint()
    {
        float Z = Random.Range(-walkpointRange, walkpointRange);
        float X = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

        if (Physics.Raycast(walkpoint, -transform.up, 2f))
        {
            walkpointSet = true;
        }
    }

    void growToast()
    {
        Instantiate(Fire, shootingPoint.position, Quaternion.LookRotation(shootingPoint.position - backRPG.position));
        var toastNOW = Instantiate(toastPrefab[Random.Range(0,3)], shootingPoint.position, Quaternion.identity);
        
        toastNOW.GetComponent<Rigidbody>().AddForce((shootingPoint.position - backRPG.position)* 150f);
        
    }
}
