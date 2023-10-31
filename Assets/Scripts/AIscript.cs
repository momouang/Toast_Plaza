using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIscript : MonoBehaviour
{

    private NavMeshAgent AI;
    public Transform[] target;
    public Transform currentTarget;
    public LayerMask isGround,toastTarget;
    public float distanceToPlayer;
    //public toastScore toastscore;

    //patrolling
    public Vector3 walkpoint;
    bool walkpointSet;
    public float walkpointRange;

    //states
    public float sightRange;

    //timer
    float currentTime;
    public float eatingTime = 0f;
    public bool isEating;
    public int pickUpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
       AI = GetComponent<NavMeshAgent>();
       pickUpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {


        if(currentTime >= eatingTime)
        {
            isEating = true;
        }

        if (currentTarget == null)
        {
            searchTarget();
            if(currentTarget == null)
            {
                patrolling();
            }         
        }

        if (currentTarget != null)
        {
            chasing();
        }
    }

    void searchTarget()
    {
        Transform near = null;
        float nearDistance = 99f;
        Collider[] scannedObjects = Physics.OverlapSphere(transform.position, sightRange, toastTarget);
        if (scannedObjects.Length != 0)
        {         
            foreach (Collider g in scannedObjects)
            {
                if(near == null)
                {
                    near = g.gameObject.transform;
                    nearDistance = Vector3.Distance(transform.position,near.position);
                }
                else if(Vector3.Distance(transform.position,g.transform.position) < nearDistance)
                {
                    near = g.gameObject.transform;
                    nearDistance = Vector3.Distance(transform.position,g.transform.position);
                }
            }
        }
        else
        {
            return;
        }
        currentTarget = near;
    }


    private void patrolling()
    {
        if(!walkpointSet)
        {
            searchwalkPoint();
        }

        if(walkpointSet)
        {
            AI.SetDestination(walkpoint);
        }

        Vector3 distancetowalkPoint = transform.position - walkpoint;

        if(distancetowalkPoint.magnitude < 1f)
        {
            walkpointSet = false;
        }
    }

    private void searchwalkPoint()
    {
        float Z = Random.Range(-walkpointRange, walkpointRange);
        float X = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z +Z);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, isGround))
        {
            walkpointSet = true;
        }
    }

    private void chasing()
    {
        AI.SetDestination(currentTarget.position);
    }

    private void OnCollisionStay(Collision collision)
    {
        currentTime += Time.deltaTime;
        if (collision.transform == currentTarget && isEating)
        {
            AI.isStopped = true;
            pickUpCount += 1;
            if(pickUpCount >= 10)
            {
                Destroy(collision.gameObject);
                pickUpCount = 0;
                isEating = false;
                AI.isStopped = false;
                currentTime = 0;
            }
               
        }
    }
}
