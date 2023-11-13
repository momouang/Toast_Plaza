using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Granny : MonoBehaviour
{
    Animator grannyAnimator;

    [Header("Shooting System")]
    AudioManager audioManager;
    public GameObject[] toastPrefab;
    public Transform shootingPoint;
    public float shootingTime = 10f;
    float currentTime;
    public ParticleSystem Fire;

    [Header("Patrolling System")]
    private NavMeshAgent grannyAi;
    public LayerMask obstacleMask;
    public Vector3 walkpoint;
    bool walkpointSet;
    public float walkpointRange;
    public Transform lookingPoint;
    public Transform backRPG;


    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        currentTime = 0;
        grannyAi = GetComponent<NavMeshAgent>();
        grannyAnimator = GetComponent<Animator>();

    }

    private void FixedUpdate()
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
        grannyAnimator.SetBool("isWalking", true); 
        if (!walkpointSet)
        {
            searchwalkPoint();
        }

        if (walkpointSet)
        {
            grannyAi.SetDestination(walkpoint);
        }

        Vector3 distancetowalkPoint = transform.position - walkpoint;

        if (distancetowalkPoint.magnitude < 5f)
        {
            walkpointSet = false;
        }

        gameObject.transform.LookAt(lookingPoint);
    }

    private void searchwalkPoint()
    {
        NavMeshHit hit;
        float Z = Random.Range(-walkpointRange, walkpointRange);
        float X = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z +Z );

        if (Physics.Raycast(walkpoint,-transform.up, 2f) && walkpoint.x > -18 && walkpoint.x < 18 && walkpoint.z > -15 && walkpoint.z < 15)
        {
            walkpointSet = true;
            //grannyAi.ResetPath();
            //Debug.Log("walkpointFalse");
        }
    }

    void growToast()
    {
        audioManager.Play("GrannyShoot");
        audioManager.Play("GrannyFire");
        Instantiate(Fire, shootingPoint.position, Quaternion.LookRotation(shootingPoint.position - backRPG.position));
        var toastNOW = Instantiate(toastPrefab[Random.Range(0,4)], shootingPoint.position, Quaternion.identity);
        
        toastNOW.GetComponent<Rigidbody>().AddForce((shootingPoint.position - backRPG.position)* 100f);
        
    }
}
