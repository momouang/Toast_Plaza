using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewKid : MonoBehaviour
{
    public enum KidState
    {
        Patrol,
        Chase,
        Idle
    }

    public AudioManager audioManager;
    Animator animator;
    public NavMeshAgent navMeshAgent;
    public Player player;
    public ParticleSystem hitParticle;
    public float speedWalk = 3;
    public float speedRun = 4; //Kid currently moves too fast.

    public float viewRadius = 15;
    public float viewAngle = 90;
    public float attackRange = .7f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public Transform[] waypoints;
    public Transform target;

    int currentWaypoint = 0;
    float WaitTime = 3f;
    float currentWaitTime = 3f;

    Vector3 playerLastPosition = Vector3.zero;

    bool playerFound = false;
    bool isWaiting = false;
    //bool isShouting = false;




    public KidState currentState = KidState.Patrol;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case KidState.Patrol:
                Patrol();
                break;

            case KidState.Chase:
                Chase();
                break;

            case KidState.Idle:
                Idle();
                break;

            default:
                Patrol();
                break;
        }
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void Stop()
    {
        animator.SetBool("isWalking", false);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        EnvironmentView();
        // if player not found, patrol to different waypoints.
        Move(speedRun);
        navMeshAgent.SetDestination(waypoints[currentWaypoint].position);   // You can use Math.Random() to decide random waypoint as destination.
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) < 1f)
        {
            // idle state;
            currentState = KidState.Idle;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        // if player is spotted, switch to Chase state;
        if (playerFound)
        {
            currentState = KidState.Chase;
            audioManager.Play("KidShout");
        }
    }

    void Chase()
    {
        animator.SetBool("isWalking", true);
        EnvironmentView();
        // if player is found, chase the player.
        if (playerFound)
        {
            // Run to player.
            Move(speedRun);
            navMeshAgent.SetDestination(playerLastPosition);
            

            // if player runs too far away, player presence is lost.
            if (Vector3.Distance(transform.position, target.position) >= 2f)
            {
                playerFound = false;
            }
            else if (Vector3.Distance(transform.position, target.position) <= attackRange)
            {
                // if player is close enough, do whatever you want to player here

            }
        }
        else
        {
            // if reach player's last known position or is stopped moving, give up and go to Idle.
            if (Vector3.Distance(transform.position, playerLastPosition) <= 1f || navMeshAgent.velocity.magnitude <= .1f)
            {
                currentState = KidState.Idle;
            }
        }
    }

    void Idle()
    {
        animator.SetBool("isWalking", false) ;
        if (currentState != KidState.Idle)
            return;

        EnvironmentView();
        Stop();
        if (!isWaiting)
        {
            WaitTimer();
        }

    }

    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform playerTransform = playerInRange[i].transform;
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerFound = true;
                    target = playerTransform;
                }
                else
                {
                    playerFound = false;
                }

                if (Vector3.Distance(transform.position, playerTransform.position) > viewRadius)
                {
                    playerFound = false;
                }
            }
            if (playerFound)
            {
                playerLastPosition = playerTransform.transform.position;
            }
        }
    }

    void WaitTimer()
    {
        // WaitTimer() is for triggering the coroutine for the first time.
        isWaiting = true;
        StartCoroutine(WaitTimerIE());
    }

    IEnumerator WaitTimerIE()
    {
        yield return new WaitForSeconds(.1f);
        currentWaitTime -= .1f;

        if (playerFound)
        {
            // if the player is found, stop idling and start chasing.
            currentWaitTime = WaitTime;
            isWaiting = false;
            currentState = KidState.Chase;
        }
        else
        {
            if (currentWaitTime >= 0)
            {
                // if the player is not found, keep idling.
                StartCoroutine(WaitTimerIE());
            }
            else
            {
                // if player is not found but it has idle far a while, go back to patrol.
                currentWaitTime = WaitTime;
                isWaiting = false;
                currentState = KidState.Patrol;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.Play("Boing");
            player.playerAnimation.SetTrigger("isFainting");
            //Debug.Log("hit");
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            Instantiate(hitParticle, pos, rot);

            float velocity = 1.5f;
            GameObject.FindObjectOfType<GameManager>().loseScore(50);
            player.controller.Move(-pos * velocity + transform.up * 2f);
        }
    }

}
