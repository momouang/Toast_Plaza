using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIscript : MonoBehaviour
{

    [Header("Main")]
    AudioManager audioManager;
    public GameObject player;
    public Player playerScript;
    Animator AiAnimator;
    private NavMeshAgent AI;
    public Transform[] target;
    public Transform currentTarget;
    public LayerMask isGround, toastTarget;
    public float distanceToPlayer;

    [Header("Partolling System")]
    public Vector3 walkpoint;
    bool walkpointSet;
    public float walkpointRange;

    [Header("States")]
    public float sightRange;

    [Header("Timer")]
    float currentTime;
    public float eatingTime = 0f;
    public bool isEating;
    public int pickUpCount = 0;

    bool isfleeing;




    private void OnEnable()
    {
        Player.Peck += OnPlayerPeck;
    }

    private void OnDisable()
    {
        Player.Peck -= OnPlayerPeck;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        AiAnimator = GetComponentInChildren<Animator>();
        AI = GetComponent<NavMeshAgent>();
        pickUpCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(distance >= 8f && isfleeing)
        {
            isfleeing = false;
            AiAnimator.SetBool("isFleeing", false);
            patrolling();
        }

        if (isfleeing)
        {
            AI.speed = 5f;
        }
        else
        {
            AI.speed = 2.5f;
        }

        //------------------------------------------

        if (currentTime >= eatingTime)
        {
            isEating = true;
        }

        if (currentTarget == null && !isfleeing)
        {
            searchTarget();
            if (currentTarget == null)
            {
                patrolling();
            }
        }

        if (currentTarget != null && !isfleeing)
        {
            chasing();
        }

        if (AI.isStopped && !isfleeing)
        {
            AiAnimator.SetBool("isWalking", false);
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
                if (near == null)
                {
                    near = g.gameObject.transform;
                    nearDistance = Vector3.Distance(transform.position, near.position);
                }
                else if (Vector3.Distance(transform.position, g.transform.position) < nearDistance)
                {
                    near = g.gameObject.transform;
                    nearDistance = Vector3.Distance(transform.position, g.transform.position);
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
        //audioManager.Play("Pigeon04");
        AiAnimator.SetBool("isWalking", true);
        if (!walkpointSet)
        {
            searchwalkPoint();
        }

        if (walkpointSet)
        {
            AI.SetDestination(walkpoint);
        }

        Vector3 distancetowalkPoint = transform.position - walkpoint;

        if (distancetowalkPoint.magnitude < 1f)
        {
            walkpointSet = false;
        }
    }

    private void searchwalkPoint()
    {
        float Z = Random.Range(-walkpointRange, walkpointRange);
        float X = Random.Range(-walkpointRange, walkpointRange);

        walkpoint = new Vector3(transform.position.x + X, transform.position.y, transform.position.z + Z);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, isGround))
        {
            walkpointSet = true;
        }
    }

    private void chasing()
    {
        AiAnimator.SetBool("isWalking", true);
        AI.SetDestination(currentTarget.position);
    }

    private void OnCollisionEnter(Collision collider)
    {
        //AiAnimator.SetBool("isWalking", false);
        currentTime += Time.deltaTime;
        if (collider.transform == currentTarget && isEating)
        {
            AiAnimator.SetTrigger("isEating");
            //pickUpCount += 1;
            audioManager.Play("AiToast");
            collider.gameObject.GetComponent<toastScore>().pickUp(false);
            //pickUpCount = 0;
            isEating = false;
            //currentTime = 0;
        }
        
    }

    void OnPlayerPeck()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= 3f)
        {
            fleeing();
        }
    }

    //fleeing
    private void fleeing()
    {
        audioManager.Play("PigeonFlee");
        isfleeing = true;
        AiAnimator.SetBool("isFleeing", true);
        //Debug.Log("fleeing");
        Vector3 dirtoPlayer = transform.position - player.transform.position;
        Vector3 pos = transform.position + dirtoPlayer;

        AI.SetDestination(pos*5);        
    }
}
